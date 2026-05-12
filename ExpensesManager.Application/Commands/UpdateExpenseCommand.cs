using ExpensesManager.Application.Responses;
using ExpensesManager.Domain.Entities;
using MediatR;

namespace ExpensesManager.Application.Commands;

public sealed record UpdateExpenseCommand(
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
    Guid? FinancialGoalId,
    decimal? GoalDeductionPercentage
) : IRequest<ExpenseResponse>;