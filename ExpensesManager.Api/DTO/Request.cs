namespace ExpensesManager.Api.DTO;

public sealed record LoginRequest(
    string Email,
    string Password);
    
public sealed record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName);