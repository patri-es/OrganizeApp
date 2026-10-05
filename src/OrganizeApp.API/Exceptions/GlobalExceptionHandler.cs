using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Application.Exceptions;
using OrganizeApp.Domain.Exceptions;

namespace OrganizeApp.API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    // TryHandleAsync permite acceder a la petición y construir la respuesta:
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Identify the type of exception
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ProductDomainException => StatusCodes.Status400BadRequest,
            CategoryDomainException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        // define title of each exception
        var title = exception switch
        {
            NotFoundException => "Resource not found",
            ProductDomainException => "Business rule violation",
            CategoryDomainException => "Business rule violation",
            _ => "Internal server error"
        };
        // define code
        var detail = statusCode == StatusCodes.Status500InternalServerError
            ? "An unexpected error occurred."
            : exception.Message;

        // format usign ProblemDetails
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        httpContext.Response.StatusCode = statusCode;
        // response with cancelation
        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;

        //var problemDetails = new ProblemDetails
        //{
        //    Title = "Ocurrió un error inesperado",
        //    Instance = httpContext.Request.Path
        //};

        //switch (exception)
        //{
        //    case NotFoundException:
        //        problemDetails.Status = StatusCodes.Status404NotFound;
        //        problemDetails.Title = "Error item not found";
        //        problemDetails.Detail = exception.Message;
        //        break;

        //    case ProductDomainException:
        //        problemDetails.Status = StatusCodes.Status400BadRequest;
        //        problemDetails.Title = "Error from Domain of item";
        //        problemDetails.Detail = exception.Message;
        //        break;

        //    case CategoryDomainException:
        //        problemDetails.Status = StatusCodes.Status400BadRequest;
        //        problemDetails.Title = "Error from Domain of item";
        //        problemDetails.Detail = exception.Message;
        //        break;

        //    default:
        //        problemDetails.Status = StatusCodes.Status500InternalServerError;
        //        problemDetails.Title = "Error of item";
        //        problemDetails.Detail = exception.Message;
        //        break;
        //}

        //return true;
    }


}
