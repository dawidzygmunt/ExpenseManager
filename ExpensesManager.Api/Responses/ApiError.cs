namespace ExpensesManager.Api.Responses;

public sealed record ApiError(
    string? Field,
    string? Message,
    string? Code
);