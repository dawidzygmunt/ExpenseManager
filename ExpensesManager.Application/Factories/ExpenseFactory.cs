using ExpensesManager.Application.Commands;
using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Application.Factories;

public class ExpenseFactory : IExpenseFactory
{
    public Expense Create(AddExpenseCommand command)
    {
        var now = DateTime.UtcNow;

        return new Expense
        {
            Id = Guid.NewGuid(),
            Amount = command.Amount,
            Type = command.Type,
            Description = command.Description,
            Date = command.Date,
            PaymentMethod = command.PaymentMethod,
            Currency = command.Currency,
            Notes = command.Notes,
            IsPeriodic = command.IsPeriodic,
            Period = command.Period,
            StartDate = command.StartDate,
            WorkspaceId = command.WorkspaceId,
            UserId = command.UserId,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
