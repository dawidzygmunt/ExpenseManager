using MediatR;

namespace ExpensesManager.Application.Commands;

public sealed record DeleteExpenseCommand(Guid Id) : IRequest;