
using ExpensesManager.Application.DTOs;

namespace ExpensesManager.Application.Responses;

public record LoginResponse(
  string AccessToken,
  string RefreshToken,
  DateTime RefreshTokenExpiryTime,
  UserDto UserDto
);