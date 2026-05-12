using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Infrastructure.Repositories;

public class ExpenseRepository(AppDbContext dbContext) : IExpenseRepository
{
    public async Task<IEnumerable<Expense>> GetAllAsync()
    {
        return await dbContext.Expenses.ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(Guid id)
    {
        return await dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Expense>> GetAllByWorkspaceIdAsync(Guid userId, Guid workspaceId)
    {
        return await dbContext.Expenses.Where(e => e.WorkspaceId == workspaceId).ToListAsync();
    }

    public async Task<Expense> AddAsync(Expense expense)
    {
        await dbContext.Expenses.AddAsync(expense);
        await dbContext.SaveChangesAsync();
        return expense;
    }

    public async Task UpdateAsync(Expense expense)
    {
        dbContext.Expenses.Update(expense);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await dbContext.Expenses.Where(e => e.Id == id).ExecuteDeleteAsync();
    }
}