using ExpensesManager.Application.Interfaces;
using ExpensesManager.Application.Queries;
using ExpensesManager.Application.Responses;
using MediatR;

namespace ExpensesManager.Application.Handlers;

public class GetExpenseByIdQueryHandler(IExpenseRepository expenseRepository)
    : IRequestHandler<GetExpenseByIdQuery, ExpenseResponse>
{
    public async Task<ExpenseResponse> Handle(
        GetExpenseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var e = await expenseRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Expense {request.Id} not found.");

        return new ExpenseResponse(
            e.Id, e.Amount, e.Type, e.Description, e.Date, e.PaymentMethod,
            e.Notes, e.Currency, e.IsPeriodic, e.Period, e.StartDate,
            e.LastProcessedDate, e.ParentExpenseId, e.WorkspaceId, e.UserId,
            e.CreatedAt, e.UpdatedAt, e.FinancialGoalId, e.GoalDeductionPercentage
        );
    }
}