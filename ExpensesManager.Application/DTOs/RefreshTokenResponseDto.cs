namespace ExpensesManager.Application.DTOs;

public record RefreshTokenResponseDto(
    string? Token,
    string? RefreshToken,
    DateTime ExpiresAt
);