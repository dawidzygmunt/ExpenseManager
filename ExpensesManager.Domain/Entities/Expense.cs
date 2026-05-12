namespace ExpensesManager.Domain.Entities;

public enum Currency
{
    PLN,
    EUR,
    USD
}

public enum ExpenseType
{
    Food,
    Transport,
    Entertainment,
    Other
}

public enum PaymentMethod
{
    Cash,
    Card,
    BankTransfer
}

public class Expense
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public ExpenseType Type { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string? Notes { get; set; }

    public Currency Currency { get; set; }

    public bool IsPeriodic { get; set; }

    public int? Period { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? LastProcessedDate { get; set; }

    public Guid? ParentExpenseId { get; set; }

    public Guid WorkspaceId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? FinancialGoalId { get; set; }

    public decimal? GoalDeductionPercentage { get; set; }
}