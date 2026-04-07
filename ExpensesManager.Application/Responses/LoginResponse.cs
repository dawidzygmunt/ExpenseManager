
using ExpensesManager.Application.DTOs;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Infrastructure.Settings;

namespace ExpensesManager.Application.Responses;

public record LoginResponse(
  string AccessToken,
  RefreshToken RefreshToken,
  UserDto UserDto
);