using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Factories;
using ExpensesManager.Application.Interfaces;
using ExpensesManager.Domain.Entities;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class AddExpenseCommandHandler(
    IExpenseRepository expenseRepository,
    IUnitOfWork unitOfWork,
    IExpenseFactory expenseFactory)
    : IRequestHandler<AddExpenseCommand, Expense>
{
    public async Task<Expense> Handle(
        AddExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var expense = expenseFactory.Create(request);

        var saved = await expenseRepository.AddAsync(expense);
        await unitOfWork.SaveChangesAsync();
        return saved;
    }
}