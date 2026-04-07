using ExpensesManager.Application.DTOs;

namespace ExpensesManager.Application.Responses;

public record RegisterResponse(
    bool Success,
    UserDto UserDto);