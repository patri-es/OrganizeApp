
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Application.Exceptions;
using OrganizeApp.Domain.Exceptions;

namespace OrganizeApp.API.Exceptions;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Identify the exception type and determine the HTTP status code.
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ProductDomainException => StatusCodes.Status400BadRequest,
            CategoryDomainException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        // Define the title for each exception type.
        var title = exception switch
        {
            NotFoundException => "Resource not found",
            ProductDomainException => "Business rule violation",
            CategoryDomainException => "Business rule violation",
            _ => "Internal server error"
        };

        // Avoid exposing internal exception details for unexpected errors.
        var detail = statusCode == StatusCodes.Status500InternalServerError
            ? "An unexpected error occurred."
            : exception.Message;

        // Create the ProblemDetails response.
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        httpContext.Response.StatusCode = statusCode;

        // Provide the ProblemDetails response to the registered service.
        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        };

        // Try to write the response using the registered ProblemDetails writers.
        var written = await _problemDetailsService.TryWriteAsync(problemDetailsContext);

        // Fallback if no registered writer supports the current request.
        if (!written)
        {
            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);
        }

        return true;
    }
}
