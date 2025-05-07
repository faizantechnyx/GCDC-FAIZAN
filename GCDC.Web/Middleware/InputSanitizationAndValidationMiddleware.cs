using Ganss.Xss;
using GCDC.Common.Helpers;

public class InputSanitizationAndValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HtmlSanitizer _sanitizer;

    public InputSanitizationAndValidationMiddleware(RequestDelegate next)
    {
        _next = next;
        _sanitizer = new HtmlSanitizer();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Sanitize and validate query parameters
        foreach (var key in context.Request.Query.Keys)
        {
            var sanitizedValue = _sanitizer.Sanitize(context.Request.Query[key]);
            if (ContainsInvalidPatterns(sanitizedValue))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid input detected.");
                return;
            }
        }

        // Sanitize and validate form data
        if (context.Request.HasFormContentType)
        {
            foreach (var key in context.Request.Form.Keys)
            {
                var sanitizedValue = _sanitizer.Sanitize(context.Request.Form[key]);
                if (AppHelper.IsValidCommand(key) && ContainsInvalidPatterns(sanitizedValue))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid input detected.");
                    return;
                }
            }
        }

        await _next(context);
    }

    private bool ContainsInvalidPatterns(string input)
    {
        // List of harmful patterns (can be expanded)
        var invalidPatterns = new[] { "--", "<script>", "union", "eval", "cmd" };
        return invalidPatterns.Any(pattern => input.ToLower().Contains(pattern));
    }
}
