using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Application.Responses;

public record ExpenseResponse(
    Guid Id,
    decimal Amount,
    ExpenseType Type,
    string Description,
    DateTime Date,
    PaymentMethod PaymentMethod,
    string? Notes,
    Currency Currency,
    bool IsPeriodic,
    int? Period,
    DateTime? StartDate,
    DateTime? LastProcessedDate,
    Guid? ParentExpenseId,
    Guid WorkspaceId,
    Guid UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    Guid? FinancialGoalId,
    decimal? GoalDeductionPercentage
);