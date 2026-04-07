namespace ExpensesManager.Api.DTO;

public sealed record LoginRequest(
    string Email,
    string Password);