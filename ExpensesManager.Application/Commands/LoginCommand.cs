using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Commands;

public sealed record LoginCommand(
  string Email,
  string Password) : IRequest<LoginResponse>;