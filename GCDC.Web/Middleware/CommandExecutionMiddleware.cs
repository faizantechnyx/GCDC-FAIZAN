using GCDC.Common.Helpers;

public class CommandExecutionMiddleware
{
    private readonly RequestDelegate _next;

    public CommandExecutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Query.ContainsKey("cmd"))
        {
            var command = context.Request.Query["cmd"];
            if (!AppHelper.IsValidCommand(command))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid command.");
                return;
            }
        }
        await _next(context);
    }
 }

