using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Commands;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<LoginResponse>;