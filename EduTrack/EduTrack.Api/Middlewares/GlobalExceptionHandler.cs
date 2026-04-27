using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Api.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Global error handler that intercepts domain exceptions and unhandled errors, 
    /// converting them into standardized Problem Details HTTP responses.
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, $"An unexpected error occurred: {exception.Message}");

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        // Domain Validations
        if (exception is ArgumentException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Bad Request";
            problemDetails.Detail = exception.Message;
        }
        // Domain Rules / Invariants
        else if (exception is InvalidOperationException)
        {
            problemDetails.Status = StatusCodes.Status409Conflict;
            problemDetails.Title = "Domain Conflict";
            problemDetails.Detail = exception.Message;
        }
        // Fallback for unexpected bugs
        else
        {
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Title = "Internal Server Error";
            problemDetails.Detail = "An internal server error occurred.";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
