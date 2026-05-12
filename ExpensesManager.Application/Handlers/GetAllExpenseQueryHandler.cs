using ExpensesManager.Application.Queries;
using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Interfaces;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class GetAllExpensesQueryHandler(IExpenseRepository expenseRepository)
    : IRequestHandler<GetAllExpensesQuery, IEnumerable<ExpenseResponse>>
{
    public async Task<IEnumerable<ExpenseResponse>> Handle(
        GetAllExpensesQuery request,
        CancellationToken cancellationToken)
    {
        var expenses = await expenseRepository.GetAllAsync();
        return expenses.Select(e => new ExpenseResponse(
            e.Id, e.Amount, e.Type, e.Description, e.Date, e.PaymentMethod,
            e.Notes, e.Currency, e.IsPeriodic, e.Period, e.StartDate,
            e.LastProcessedDate, e.ParentExpenseId, e.WorkspaceId, e.UserId,
            e.CreatedAt, e.UpdatedAt, e.FinancialGoalId, e.GoalDeductionPercentage
        ));
    }
}