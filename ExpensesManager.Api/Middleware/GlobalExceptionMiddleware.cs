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
            AppException appException => Fail(appException.StatusCode, appException.Errors),
            UnauthorizedAccessException => Fail(StatusCodes.Status401Unauthorized, exception.Message),
            ArgumentException => Fail(StatusCodes.Status400BadRequest, exception.Message),
            InvalidOperationException => Fail(StatusCodes.Status400BadRequest, exception.Message),
            _ => BuildUnhandledResponse(exception)
        };

        if (response.Status >= StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception while processing {Method} {Path}",
                context.Request.Method, context.Request.Path);
        else
            logger.LogWarning(exception, "Handled exception while processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

        context.Response.Clear();
        context.Response.StatusCode = response.Status;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsJsonAsync(response);
    }

    private ApiResponse<object> BuildUnhandledResponse(Exception exception)
    {
        var message = environment.IsDevelopment()
            ? exception.Message
            : "An unexpected error occurred.";
        return Fail(StatusCodes.Status500InternalServerError, message);
    }

    private static ApiResponse<object> Fail(int status, string message)
    {
        return new ApiResponse<object>(false, null, status, message,
            new[] { new ApiError(null, message, null) });
    }

    private static ApiResponse<object> Fail(int status, IEnumerable<string> errors)
    {
        var list = errors.Select(e => new ApiError(null, e, null)).ToList();
        return new ApiResponse<object>(false, null, status, list.FirstOrDefault()?.Message, list);
    }
}