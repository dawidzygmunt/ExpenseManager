using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Queries;

public sealed record GetExpensesByWorkspaceIdQuery(
    Guid UserId,
    Guid WorkspaceId) : IRequest<IEnumerable<ExpenseResponse>>;