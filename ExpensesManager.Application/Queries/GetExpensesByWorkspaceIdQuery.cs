using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Queries;

public sealed record GetWorkspaceExpensesQuery(
    Guid UserId,
    Guid WorkspaceId) : IRequest<ExpenseResponse>;