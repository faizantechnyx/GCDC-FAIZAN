public class WAFMiddleware
{
    private readonly RequestDelegate _next;

    public WAFMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // List of suspicious patterns
        var maliciousPatterns = new[] { "union", "drop", "exec", "cmd", "<script>", "eval" };

        // Whitelist for Umbraco TinyMCE or safe patterns
        var safePatterns = new[]
        {
            "/umbraco/backoffice/",       // Example: Umbraco backoffice APIs
            "/umbraco/api/",              // Example: Umbraco APIs
            "tinymce"                     // Example: TinyMCE-related keywords
        };

        // Extract the request content to inspect
        var requestContent = context.Request.QueryString.ToString().ToLower() +
                             context.Request.Path.ToString().ToLower();

        // Skip WAF checks for safe patterns
        if (safePatterns.Any(pattern => requestContent.Contains(pattern)))
        {
            await _next(context);
            return;
        }

        // Check for malicious patterns
        if (maliciousPatterns.Any(pattern => requestContent.Contains(pattern)))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Malicious request blocked.");
            return;
        }

        // Proceed with the next middleware
        await _next(context);
    }

}
