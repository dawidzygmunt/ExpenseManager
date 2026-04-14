using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Application.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user, IList<string> roles);
    RefreshToken GenerateRefreshToken();
    (string jti, DateTime expiry) DecodeAccessToken(string accessToken);
}