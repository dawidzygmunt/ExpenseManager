namespace ExpensesManager.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}