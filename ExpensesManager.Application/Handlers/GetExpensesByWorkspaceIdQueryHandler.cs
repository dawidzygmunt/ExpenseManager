using ExpensesManager.Application.Interfaces;
using ExpensesManager.Application.Queries;
using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class GetExpensesByWorkspaceIdQueryHandler(IExpenseRepository expenseRepository)
    : IRequestHandler<GetExpensesByWorkspaceIdQuery, IEnumerable<ExpenseResponse>>
{
    public async Task<IEnumerable<ExpenseResponse>> Handle(
        GetExpensesByWorkspaceIdQuery request,
        CancellationToken cancellationToken)
    {
        var expenses = await expenseRepository.GetAllByWorkspaceIdAsync(request.UserId, request.WorkspaceId);
        return expenses.Select(e => new ExpenseResponse(
            e.Id, e.Amount, e.Type, e.Description, e.Date, e.PaymentMethod,
            e.Notes, e.Currency, e.IsPeriodic, e.Period, e.StartDate,
            e.LastProcessedDate, e.ParentExpenseId, e.WorkspaceId, e.UserId,
            e.CreatedAt, e.UpdatedAt, e.FinancialGoalId, e.GoalDeductionPercentage
        ));
    }
}