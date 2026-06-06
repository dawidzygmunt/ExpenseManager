using ExpensesManager.Application.Commands;
using ExpensesManager.Application.Interfaces;
using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class UpdateExpenseCommandHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateExpenseCommand, ExpenseResponse>
{
    public async Task<ExpenseResponse> Handle(
        UpdateExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var expense = await expenseRepository.GetByIdAsync(request.Id);


        expense.Amount = request.Amount;
        expense.Type = request.Type;
        expense.Description = request.Description;
        expense.Date = request.Date;
        expense.PaymentMethod = request.PaymentMethod;
        expense.Notes = request.Notes;
        expense.Currency = request.Currency;
        expense.IsPeriodic = request.IsPeriodic;
        expense.Period = request.Period;
        expense.StartDate = request.StartDate;
        expense.FinancialGoalId = request.FinancialGoalId;
        expense.GoalDeductionPercentage = request.GoalDeductionPercentage;
        expense.UpdatedAt = DateTime.UtcNow;

        expenseRepository.Update(expense);
        await unitOfWork.SaveChangesAsync();


        return new ExpenseResponse(
            expense.Id,
            expense.Amount,
            expense.Type,
            expense.Description,
            expense.Date,
            expense.PaymentMethod,
            expense.Notes,
            expense.Currency,
            expense.IsPeriodic,
            expense.Period,
            expense.StartDate,
            expense.LastProcessedDate,
            expense.ParentExpenseId,
            expense.WorkspaceId,
            expense.UserId,
            expense.CreatedAt,
            expense.UpdatedAt,
            expense.FinancialGoalId,
            expense.GoalDeductionPercentage
        );
    }
}