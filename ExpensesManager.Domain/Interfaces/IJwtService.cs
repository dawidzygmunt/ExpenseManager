using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Domain.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user, List<string> roles);
}