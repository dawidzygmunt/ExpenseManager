
using ExpensesManager.Application.DTOs;

namespace ExpensesManager.Application.Responses;

public record LoginResponse(
  string AccessToken,
  UserDto UserDto
);