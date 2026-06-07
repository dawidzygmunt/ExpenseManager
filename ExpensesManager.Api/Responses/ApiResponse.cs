namespace ExpensesManager.Api.Responses;

public sealed record ApiResponse<T>(
    bool Success,
    T? Data,
    int Status,
    string? Message,
    IReadOnlyList<ApiError>? Errors)
{
    public static ApiResponse<T> Ok(T data, int status = 200, string? message = null)
        => new(true, data, status, message, null);

    public static ApiResponse<T> Fail(int status, string message, IReadOnlyList<ApiError>? errors = null)
        => new(false, default, status, message, errors);
}