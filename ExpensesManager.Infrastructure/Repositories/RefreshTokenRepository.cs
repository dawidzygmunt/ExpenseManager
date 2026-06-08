using ExpensesManager.Application.Interfaces;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext dbContext) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken token)
    {
        await dbContext.RefreshTokens.AddAsync(token);
    }

    public Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
    }

    public void Update(RefreshToken token)
    {
        dbContext.RefreshTokens.Update(token);
    }
}