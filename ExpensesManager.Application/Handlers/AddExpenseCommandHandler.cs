using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Interfaces;
using ExpensesManager.Domain.Entities;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class AddExpenseCommandHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AddExpenseCommand, Expense>
{
    public async Task<Expense> Handle(
        AddExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Type = request.Type,
            Description = request.Description,
            Date = request.Date,
            PaymentMethod = request.PaymentMethod,
            Currency = request.Currency,
            Notes = request.Notes,
            IsPeriodic = request.IsPeriodic,
            Period = request.Period,
            StartDate = request.StartDate,
            WorkspaceId = request.WorkspaceId,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var saved = await expenseRepository.AddAsync(expense);
        await unitOfWork.SaveChangesAsync();
        return saved;
    }
}