using System.Text;
using System.Text.RegularExpressions;

public class MaliciousCodeDetectionMiddleware
{
    private readonly RequestDelegate _next;

    // List of malicious patterns (you can expand this list)
    private static readonly string[] _maliciousPatterns = {
        @"Process\.Start\(",
        @"System\.Diagnostics\.Process",
        @"cmd\.exe",
        @"powershell\.exe",
        @"Request\.Query\[""cmd""\]",
        @"RedirectStandardOutput",
        @"UseShellExecute\s*=\s*false",
        @"CreateNoWindow",
        @"System\.Reflection\.Assembly",
        @"System\.IO\.File\.WriteAllText",
        @"System\.IO\.File\.ReadAllText",
        @"eval\(",
        @"exec\(",
        @"shell_exec\(",
        @"passthru\(",
        @"base64_decode\(",
        @"preg_replace\(.*/e"
    };

    public MaliciousCodeDetectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Capture the original response body stream
        var originalBodyStream = context.Response.Body;

        // Create a new MemoryStream to capture the response
        using var memoryStream = new MemoryStream();

        // Replace the response body with the MemoryStream
        context.Response.Body = memoryStream;

        // Call the next middleware in the pipeline
        await _next(context);

        // Reset the position of the MemoryStream to read its content
        memoryStream.Seek(0, SeekOrigin.Begin);

        // Read the response content from the MemoryStream
        var responseText = await new StreamReader(memoryStream, Encoding.UTF8).ReadToEndAsync();

        // Check for malicious patterns in the response
        if (ContainsMaliciousCode(responseText))
        {
            // Modify the response if malicious code is detected
            responseText = "Malicious request blocked.";
            context.Response.StatusCode = StatusCodes.Status403Forbidden; // Set HTTP status code to 403 Forbidden

            // Convert the modified response text back to bytes
            var responseBytes = Encoding.UTF8.GetBytes(responseText);

            // Restore the original response body stream
            context.Response.Body = originalBodyStream;

            // Write the modified response back to the original response stream
            await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
        else
        {
            // If no malicious content is detected, restore the original response body stream
            context.Response.Body = originalBodyStream;

            // Reset the position of the MemoryStream to copy it back to the original stream
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Copy the original response back to the client without re-encoding
            await memoryStream.CopyToAsync(originalBodyStream);
        }
    }

    private bool ContainsMaliciousCode(string content)
    {
        foreach (var pattern in _maliciousPatterns)
        {
            if (Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase))
            {
                // Log the detection of malicious code
                Console.WriteLine($"Malicious code detected: Pattern '{pattern}' matched.");
                return true; // Malicious code detected
            }
        }
        return false; // No malicious code found
    }

}
