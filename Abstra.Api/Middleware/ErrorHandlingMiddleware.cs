using System.Net;
using System.Text.Json;

namespace Abstra.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var message = "An error occurred while processing your request.";

        if (exception is InvalidOperationException)
        {
            code = HttpStatusCode.BadRequest;
            message = exception.Message;
        }
        else if (exception is UnauthorizedAccessException)
        {
            code = HttpStatusCode.Unauthorized;
            message = "Unauthorized access.";
        }
        else if (exception is KeyNotFoundException || exception is ArgumentNullException)
        {
            code = HttpStatusCode.NotFound;
            message = exception.Message;
        }

        var result = JsonSerializer.Serialize(new { error = message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
