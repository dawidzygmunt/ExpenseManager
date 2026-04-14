using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Domain.Interfaces;

public interface IUserRepository
{
  Task<User?> GetByEmailAsync(string email);
  Task<User> AddAsync(User user, string password, string role="User");
  Task<bool> CheckPasswordAsync(User user, string password);
  Task<IList<string>> GetRolesAsync(User user);
  Task AddToRoleAsync(User user, string role);
  Task RemoveFromRoleAsync(User user, string role);
}