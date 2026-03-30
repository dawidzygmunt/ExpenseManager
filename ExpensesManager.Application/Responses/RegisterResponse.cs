using ExpensesManager.Application.Dtos;

namespace ExpensesManager.Application.Responses;

public record RegisterResponse(
    bool Success,
    UserDto UserDto);