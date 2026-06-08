using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    void Update(RefreshToken token);
}