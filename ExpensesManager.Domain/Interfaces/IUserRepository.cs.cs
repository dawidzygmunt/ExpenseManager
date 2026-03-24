using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Domain.Interfaces;

public interface IUserRepository
{
  Task<User?> GetEmailAsync(string email);
}