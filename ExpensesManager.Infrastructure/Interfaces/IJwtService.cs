using ExpensesManager.Domain.Entities;
using ExpensesManager.Infrastructure.Settings;

namespace ExpensesManager.Infrastructure.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken();
    (string jti, DateTime expiry) DecodeAccessToken(string accessToken);
}