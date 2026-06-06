using ExpensesManager.Application.Interfaces;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Infrastructure.Repositories;

public class BlackListRepository(AppDbContext dbContext) : IBlackListRepository
{
    public async Task AddAsync(BlackListedToken token)
    {
        await dbContext.BlackListedTokens.AddAsync(token);
    }

    public async Task<bool> IsBlacklistedAsync(string jti)
    {
        return await dbContext.BlackListedTokens.AnyAsync(u => u.Jti == jti);
    }

    public async Task RemoveExpiredAsync()
    {
        await dbContext.BlackListedTokens.Where(t => t.Expiry < DateTime.UtcNow).ExecuteDeleteAsync();
    }
}