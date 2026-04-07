using MediatR;

namespace ExpensesManager.Application.Commands;

public sealed record LogoutCommand(
    string AccessToken
) : IRequest<Unit>;