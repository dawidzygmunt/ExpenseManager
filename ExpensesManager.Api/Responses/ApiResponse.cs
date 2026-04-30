namespace ExpensesManager.Api.Responses;

public sealed record ApiResponse(
    int StatusCode,
    object? Data,
    IReadOnlyList<string>? Errors)
{
    public static ApiResponse Success(object? data, int statusCode = 200)
    {
        return new ApiResponse(statusCode, data, null);
    }

    public static ApiResponse Failure(int statusCode, IEnumerable<string> errors)
    {
        return new ApiResponse(statusCode, null, errors.ToList());
    }

    public static ApiResponse Failure(int statusCode, string error)
    {
        return new ApiResponse(statusCode, null, new[] { error });
    }
}