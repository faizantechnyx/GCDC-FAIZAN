using GCDC.Common.Helpers;

namespace GCDC.Web.Web.Middleware
{
	public class RouteRedirectionMiddleware
	{
		private readonly RequestDelegate _next;

		public RouteRedirectionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var path = context.Request.Path;
			var queryString = context.Request.QueryString.Value; // Preserve query string
			var reservedUrls = new List<string>
			{
				"/umbraco",
				"/umbidlocallogin",
				"/umbraco-signin-oidc",
				"/App_Plugins",
				"/api",
				"/sitemap",
				"/forms/get-contactus-form-data",
				"/forms/submit-career-form",
				"/forms/submit-contactus-form"
			};

			string fileExtension = Path.GetExtension(path);
			if (string.IsNullOrEmpty(fileExtension))
			{
				string pathString = path.ToString(); // Convert PathString to string for comparisons

				// Check if it's a root request and redirect to /en/
				if (string.IsNullOrEmpty(pathString) || pathString == "/")
				{
					string redirectUrl = $"/en/{queryString}";
					if (!pathString.Equals(redirectUrl, StringComparison.OrdinalIgnoreCase)) // Prevent loop
					{
						context.Response.Redirect(redirectUrl);
						return;
					}
				}

				// Ensure a trailing slash for specific language URLs
				if ((pathString.StartsWith("/en", StringComparison.OrdinalIgnoreCase) || pathString.StartsWith("/ar", StringComparison.OrdinalIgnoreCase)) && !pathString.EndsWith("/"))
				{
					string redirectUrl = $"{pathString}/{queryString}";
					if (!pathString.Equals(redirectUrl, StringComparison.OrdinalIgnoreCase)) // Prevent loop
					{
						context.Response.Redirect(redirectUrl);
						return;
					}
				}

				foreach (var key in context.Request.Query.Keys)
				{
					if (!context.Request.Path.ToString().Contains("/Umbraco".ToLower()) && !AppHelper.IsValidCommand(key))
					{
						context.Response.StatusCode = StatusCodes.Status400BadRequest;
						await context.Response.WriteAsync("Invalid input detected.");
						return;
					}
				}

				// Check for reserved URLs
				bool isReservedUrl = reservedUrls.Any(url => pathString.StartsWith(url, StringComparison.OrdinalIgnoreCase));

				if (!isReservedUrl)
				{
					// Redirect non-reserved URLs to /en/ if they don't already have a language prefix
					if (!pathString.StartsWith("/en/", StringComparison.OrdinalIgnoreCase) &&
						!pathString.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase))
					{
						// Add trailing slash if it doesn't exist
						string redirectPath = $"/en{pathString}";
						if (!redirectPath.EndsWith("/"))
						{
							redirectPath += "/";
						}

						string redirectUrl = $"{redirectPath}{queryString}";
						if (!pathString.Equals(redirectUrl, StringComparison.OrdinalIgnoreCase)) // Prevent loop
						{
							context.Response.Redirect(redirectUrl);
							return;
						}
					}
				}
			}

			// Continue processing the request
			await _next(context);
		}
	}

}
