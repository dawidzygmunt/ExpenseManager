using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task UpdateAsync(RefreshToken token);
}