using ExpensesManager.Application.Interfaces;
using ExpensesManager.Infrastructure.Data;

namespace ExpensesManager.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}