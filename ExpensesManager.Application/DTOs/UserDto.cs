
namespace ExpensesManager.Application.DTOs;

public sealed record UserDto(
  Guid Id,
  string Email,
  string FirstName,
  string LastName
);