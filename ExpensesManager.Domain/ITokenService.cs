using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Domain.Interfaces;

public interface ITokenService
{
  string GenerateToken(User user);
}