using ExpensesManager.Api.Responses;
using ExpensesManager.Application.Exceptions;

namespace ExpensesManager.Api.Middleware;

public class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = exception switch
        {
            AppException appException => ApiResponse.Failure(appException.StatusCode, appException.Errors),
            UnauthorizedAccessException => ApiResponse.Failure(StatusCodes.Status401Unauthorized,
                exception.Message),
            ArgumentException => ApiResponse.Failure(StatusCodes.Status400BadRequest, exception.Message),
            InvalidOperationException => ApiResponse.Failure(StatusCodes.Status400BadRequest,
                exception.Message),
            _ => BuildUnhandledResponse(exception)
        };

        if (response.StatusCode >= StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception while processing {Method} {Path}",
                context.Request.Method, context.Request.Path);
        else
            logger.LogWarning(exception, "Handled exception while processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

        context.Response.Clear();
        context.Response.StatusCode = response.StatusCode;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsJsonAsync(response);
    }

    private ApiResponse BuildUnhandledResponse(Exception exception)
    {
        var message = environment.IsDevelopment()
            ? exception.Message
            : "An unexpected error occurred.";
        return ApiResponse.Failure(StatusCodes.Status500InternalServerError, message);
    }
}