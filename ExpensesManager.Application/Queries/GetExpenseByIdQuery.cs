using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Queries;

public sealed record GetExpenseByIdQuery(
    Guid Id
) : IRequest<ExpenseResponse>;