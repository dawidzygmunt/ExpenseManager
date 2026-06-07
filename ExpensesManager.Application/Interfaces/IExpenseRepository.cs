using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Application.Interfaces;

public interface IExpenseRepository
{
    Task<IEnumerable<Expense>> GetAllAsync();
    Task<Expense?> GetByIdAsync(Guid id);
    Task<IEnumerable<Expense>> GetAllByWorkspaceIdAsync(Guid userId, Guid workspaceId);
    Task<Expense> AddAsync(Expense expense);
    void Update(Expense expense);
    Task DeleteAsync(Guid id);
}