using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Api.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Attempts to handle the specified exception by generating an appropriate HTTP response with problem details.
    /// </summary>
    /// <remarks>The method maps <see cref="ArgumentException"/> to a 400 Bad Request response and <see
    /// cref="InvalidOperationException"/> to a 409 Conflict response. All other exceptions result in a 500 Internal
    /// Server Error response. The response body contains a problem details object describing the error.</remarks>
    /// <param name="httpContext">The HTTP context for the current request. Used to write the response.</param>
    /// <param name="exception">The exception to handle. Determines the type of problem details and HTTP status code returned.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A value task that represents the asynchronous operation. The result is <see langword="true"/> if the exception
    /// was handled and a response was written; otherwise, <see langword="false"/>.</returns>
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

        if (exception is ArgumentException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Bad Request";
            problemDetails.Detail = exception.Message;
        }
        else if (exception is InvalidOperationException)
        {
            problemDetails.Status = StatusCodes.Status409Conflict;
            problemDetails.Title = "Domain Conflict";
            problemDetails.Detail = exception.Message;
        }
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
