using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext dbContext) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken token)
    {
        await dbContext.RefreshTokens.AddAsync(token);
        await dbContext.SaveChangesAsync();
    }

    public Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task UpdateAsync(RefreshToken token)
    {
        dbContext.RefreshTokens.Update(token);
        await dbContext.SaveChangesAsync();
    }
}