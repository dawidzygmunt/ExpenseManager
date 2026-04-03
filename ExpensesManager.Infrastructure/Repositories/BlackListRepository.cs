using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Infrastructure.Repositories;

public class BlackListRepository(AppDbContext dbContext) : IBlackListRepository
{
    public async Task AddAsync(BlackListedToken token)
    {
        await dbContext.BlackListedTokens.AddAsync(token);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsBlacklistedAsync(string jti)
    {
        return await dbContext.BlackListedTokens.AnyAsync(u => u.Jti == jti);
    }
}