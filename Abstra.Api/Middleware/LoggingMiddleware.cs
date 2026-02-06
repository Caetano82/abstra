using System.Diagnostics;

namespace Abstra.Api.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;

        _logger.LogInformation(
            "Incoming request: {Method} {Path}",
            requestMethod,
            requestPath);

        await _next(context);

        stopwatch.Stop();
        var statusCode = context.Response.StatusCode;

        _logger.LogInformation(
            "Completed request: {Method} {Path} - Status: {StatusCode} - Duration: {ElapsedMilliseconds}ms",
            requestMethod,
            requestPath,
            statusCode,
            stopwatch.ElapsedMilliseconds);
    }
}
