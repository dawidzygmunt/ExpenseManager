using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Commands;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<RegisterResponse>;