using Serilog;
using AspNetCoreRateLimit;
using GCDC.Common.Helpers;
using GCDC.Core.Repositories.Common;
using System.Text.Json.Serialization;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.FileProviders;
using GCDC.Web.Web.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to remove the Server header (which contains version info)
builder.WebHost.ConfigureKestrel(options =>
{
	// Disable the server header to hide version info
	options.AddServerHeader = false;
});

builder.Services.AddControllersWithViews();

// Add Memory Cache (Required for Rate Limiting)
builder.Services.AddMemoryCache();

// Add Rate Limiting Services
builder.Services.Configure<IpRateLimitOptions>(options =>
{
	options.GeneralRules = new List<RateLimitRule>
	{
		new RateLimitRule
		{
			Endpoint = "*", // Apply to all endpoints
            Limit = 500,    // Allow 500 requests
            Period = "1m"   // Per minute
        }
	};

	// Whitelist Umbraco Backoffice Endpoints to Prevent Login Issues
	options.EndpointWhitelist = new List<string>
	{
		"/umbraco",
		"/umbraco/backoffice/*",
		"/umbraco/api/*",
		"/umbraco/preview/*",
		"/umbraco/BackOffice/UmbracoApi/Authentication/PostLogin",
		"/umbraco/BackOffice/UmbracoApi/Authentication/GetCurrentUser",
		"/umbraco/BackOffice/UmbracoApi/Authentication/PostLogout",
		"/umbraco/BackOffice/UmbracoApi/EntitySearch/GetResultForSearch",
		"/umbraco/BackOffice/UmbracoApi/Content/GetById",
		"/umbraco/BackOffice/UmbracoApi/Dashboard/GetDashboard"
	};
});

// Register In-Memory Rate Limiting
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


builder.Services.AddRazorPages();

// Configure Serilog from appsettings.json
var config = builder.Configuration;
var env = builder.Environment;

config
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Azure Key Vault configuration
var keyVaultUrl = config["KeyVaultSettings:URL"];
if (!string.IsNullOrEmpty(keyVaultUrl) && Uri.TryCreate(keyVaultUrl, UriKind.Absolute, out var validUri))
{
    TokenCredential credential = env.IsDevelopment()
        ? new ClientSecretCredential(config["KeyVaultSettings:ClientTenantId"], config["KeyVaultSettings:ClientId"], config["KeyVaultSettings:ClientSecret"])
        : new DefaultAzureCredential();
    var secretClient = new SecretClient(validUri, credential);
    config.AddAzureKeyVault(validUri, credential);
}



// Clear other logging providers
builder.Logging.ClearProviders();

builder.Host.UseSerilog(Log.Logger);

// Increase max response size (for example, to 10 MB)
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
    serverOptions.Limits.MaxResponseBufferSize = 10 * 1024 * 1024; // 10 MB
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});


// 1. Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins()    // Allows requests from specific origins
				   .AllowAnyHeader()   // Allows any headers
                   .AllowAnyMethod();  // Allows any HTTP method
        });
});

builder.Services.AddHttpContextAccessor();

if (env.IsDevelopment())
{
    builder.CreateUmbracoBuilder()
        .AddBackOffice()
        .AddWebsite()
        .AddComposers()
        .Build();
}
else
{
    builder.CreateUmbracoBuilder()
        .AddBackOffice()
        .AddWebsite()
        .AddComposers()
        .AddAzureBlobMediaFileSystem()
        .AddAzureBlobImageSharpCache()
        .Build();
}

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.MaxDepth = 100; // Adjust depth if needed
    });

//Initialize Logger
Common.Initialize(config);

//Factory Design Pattern
builder.Services.AddScoped<IBaseControllerFactory, BaseControllerFactory>();

WebApplication app = builder.Build();

app.UseStaticFiles();

// Security headers
app.Use(async (context, next) =>
{

	context.Response.Headers.Remove("Server");
	context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
	context.Response.Headers["X-Content-Type-Options"] = "nosniff";
	context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
	context.Response.Headers["Referrer-Policy"] = "same-origin";
	context.Response.Headers["Permissions-Policy"] = "fullscreen=(self)";
	context.Response.Headers.Add("Content-Security-Policy",
	   "form-action 'self'; " +
       "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://code.jquery.com https://www.google.com https://www.google-analytics.com https://maps.googleapis.com https://www.gstatic.com https://www.googletagmanager.com https://img08.en25.com;" +
	   "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com ; " +
	   "worker-src 'self' blob:; " +
	   "connect-src 'self' https://www.google-analytics.com https://maps.googleapis.com https://www.google.com;");

	context.Response.Headers["X-XSS-Protection"] = "1; mode=block";


	// whitelist of allowed hosts
	var allowedHosts = new List<string>();
	var allowedHostsString = builder.Configuration["AllowedCustomHosts"];
	if (!string.IsNullOrEmpty(allowedHostsString))
	{
		allowedHosts = allowedHostsString.Split(',').Select(h => h.Trim()).ToList();
	}

	// Validate Host or X-Forwarded-Host header
	var forwardedHost = context.Request.Headers["X-Forwarded-Host"].FirstOrDefault();
	var host = forwardedHost ?? context.Request.Host.Host;

	// Reject requests with invalid Host or X-Forwarded-Host
	if (!allowedHosts.Contains(host))
	{
		context.Response.StatusCode = 400; // Bad Request
		await context.Response.WriteAsync("Invalid host header.");
		return;
	}

	// Validate Host or X-Forwarded-For header
	var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
	var _for = forwardedFor ?? context.Request.Host.Host;

	await next();
});

await app.BootUmbracoAsync();

// Use compression middleware
app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseMiddleware<RouteRedirectionMiddleware>();  // Route Redirection validation

app.UseHsts();
app.UseCookiePolicy(new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.Strict,
	Secure = CookieSecurePolicy.Always
});

app.Use(async (context, next) =>
{
	if (context.Response.StatusCode == StatusCodes.Status301MovedPermanently)
	{
		// Add HSTS header to the redirect response
		context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
	}
	await next();
});

app.UseMiddleware<MaliciousCodeDetectionMiddleware>(); // MaliciousCodeDetector
app.UseMiddleware<WAFMiddleware>(); // Web Application Firewall
app.UseMiddleware<InputSanitizationAndValidationMiddleware>(); // Input sanitization and validation
app.UseMiddleware<CommandExecutionMiddleware>(); // Command validation

// Serve files in App_Plugins
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(Directory.GetCurrentDirectory(), "App_Plugins")),
	RequestPath = "/App_Plugins"
});


app.UseRouting();
app.UseAuthentication();

// Apply Rate Limiting Middleware
app.UseIpRateLimiting();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

app.UseCors(builder =>
    builder.WithOrigins()
           .AllowAnyMethod()
           .AllowAnyHeader());

await app.RunAsync();
