using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class AddExpenseCommandHandler(IExpenseRepository expenseRepository)
    : IRequestHandler<AddExpenseCommand, ExpenseResponse>
{
    public async Task<ExpenseResponse> Handle(
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
        return new ExpenseResponse(
            saved.Id,
            saved.Amount,
            saved.Type,
            saved.Description,
            saved.Date,
            saved.PaymentMethod,
            saved.Notes,
            saved.Currency,
            saved.IsPeriodic,
            saved.Period,
            saved.StartDate,
            saved.LastProcessedDate,
            saved.ParentExpenseId,
            saved.WorkspaceId,
            saved.UserId,
            saved.CreatedAt,
            saved.UpdatedAt,
            saved.FinancialGoalId,
            saved.GoalDeductionPercentage
        );
    }
}