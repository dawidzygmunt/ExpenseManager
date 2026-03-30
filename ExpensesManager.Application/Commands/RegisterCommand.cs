using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Commands;

public sealed record RegisterCommand(
    string Email,
    string Password) : IRequest<RegisterResponse>;