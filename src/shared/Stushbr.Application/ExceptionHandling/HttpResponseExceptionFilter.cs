using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Stushbr.Application.ExceptionHandling;

public class HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var exception = context.Exception;

        switch (exception)
        {
            case null:
                return;
            case HttpException ex:
                HandleHttpExceptionAsync(context, ex);
                break;
            default:
                HandleInternalException(context, exception);
                break;
        }

        context.ExceptionHandled = true;
    }

    private void HandleInternalException(ActionExecutedContext context, Exception exception)
    {
        var httpContext = context.HttpContext;

        logger.LogError(
            new EventId(),
            exception,
            "Handling internal exception ({ExceptionTypeName}).\n" +
            "Request: {RequestMethod} {RequestPath}\n",
            exception.GetType().Name,
            httpContext.Request.Method,
            httpContext.Request.Path
        );

        context.Result = new JsonResult(new ErrorsResponse("Internal Server Error"))
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }

    private void HandleHttpExceptionAsync(ActionExecutedContext context, HttpException exception)
    {
        var httpContext = context.HttpContext;

        logger.LogWarning(
            "Handling HttpException ({ExceptionTypeName}).\n" +
            "Request: {RequestMethod} {RequestPath}\n" +
            "Message: {Message}",
            exception.GetType().Name,
            httpContext.Request.Method,
            httpContext.Request.Path,
            exception.Message
        );

        context.Result = new JsonResult(exception.Response)
        {
            StatusCode = (int)exception.HttpCode
        };
    }
}
