using ExpensesManager.Application.DTOs;

namespace ExpensesManager.Application.Responses;

public record LoginResponse(
    string Token,
    string RefreshToken,
    DateTime RefreshTokenExpiryTime,
    UserDto UserDto
);