using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Product.Application.Exceptions;

namespace Product.Application.Middlewares;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        logger.LogError(exception,
            "Could not process a request on machine {MachineName}. TraceId: {TraceId}",
            Environment.MachineName,
            traceId);

        await GenerateProblemDetails(httpContext, traceId, exception);

        return true;
    }

    private static async Task GenerateProblemDetails(HttpContext httpContext,
        string traceId,
        Exception exception)
    {
        var (statusCode, title) = MapExceptionWithStatusCode(exception);

        await Results.Problem(title: title,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                {
                    "traceId", traceId
                }
            }).ExecuteAsync(httpContext);
    }

    private static (int statusCode, string title) MapExceptionWithStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest,
                exception.Message),
            ValidationException => (StatusCodes.Status400BadRequest,
                GenerateFluentValidationErrorMessage((exception as ValidationException)!.Failures)),
            KeyNotFoundException => (StatusCodes.Status404NotFound,
                exception.Message),
            _ => (StatusCodes.Status500InternalServerError,
                "We are sorry for the inconvenience but we are on it.")
        };
    }

    private static string GenerateFluentValidationErrorMessage(
        IDictionary<string, string[]> failures)
    {
        return string.Join("; ",
            failures.Select(failure =>
                $"{failure.Key}: {string.Join(", ", failure.Value)}"));
    }
}