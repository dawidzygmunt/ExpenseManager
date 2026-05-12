using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Queries;

public sealed record GetAllExpensesQuery : IRequest<IEnumerable<ExpenseResponse>>;