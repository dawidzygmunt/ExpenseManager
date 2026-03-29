
namespace ExpensesManager.Application.Dtos;

public sealed record UserDto(
  Guid Id,
  string Email,
  string FirstName,
  string LastName
);