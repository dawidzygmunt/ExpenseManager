using ExpensesManager.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpensesManager.Api.Filters;

public sealed class ApiResponseFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult)
        {
            var status = objectResult.StatusCode ?? StatusCodes.Status200OK;
            var value = objectResult.Value;

            if (value is not null && !IsAlreadyWrapped(value))
            {
                var wrapperType = typeof(ApiResponse<>).MakeGenericType(value.GetType());
                var wrapped = Activator.CreateInstance(
                    wrapperType, true, value, status, null, null);

                objectResult.Value = wrapped;
                objectResult.DeclaredType = wrapperType;
            }
        }
        else if (context.Result is StatusCodeResult statusCodeResult &&
                 statusCodeResult.StatusCode is StatusCodes.Status204NoContent)
        {
            context.Result = new ObjectResult(
                new ApiResponse<object?>(true, null, 200, null, null))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        await next();
    }

    private static bool IsAlreadyWrapped(object value)
    {
        var type = value.GetType();
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ApiResponse<>);
    }
}