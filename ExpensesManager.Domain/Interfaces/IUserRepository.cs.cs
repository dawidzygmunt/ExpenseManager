using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Domain.Interfaces;

public interface IUserRepository
{
  Task<User?> GetByEmailAsync(string email);
  Task<User> AddAsync(User user, string password);
  Task<bool> CheckPasswordAsync(User user, string password);
}