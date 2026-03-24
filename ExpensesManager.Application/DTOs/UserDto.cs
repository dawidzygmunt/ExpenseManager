
namespace ExpensesManager.Application.Dto;

public sealed record UserDto(
  Guid Id,
  string Email,
  string FirstName,
  string LastName
);