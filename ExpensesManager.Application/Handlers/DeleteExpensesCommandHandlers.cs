using ExpensesManager.Application.Commands;
using ExpensesManager.Domain.Interfaces;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class DeleteExpenseCommandHandler(IExpenseRepository expenseRepository)
    : IRequestHandler<DeleteExpenseCommand>
{
    public async Task Handle(
        DeleteExpenseCommand request,
        CancellationToken cancellationToken)
    {
        await expenseRepository.DeleteAsync(request.Id);
    }
}