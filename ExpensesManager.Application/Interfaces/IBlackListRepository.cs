using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Application.Interfaces;

public interface IBlackListRepository
{
    Task AddAsync(BlackListedToken token);
    Task<bool> IsBlacklistedAsync(string jti);
    Task RemoveExpiredAsync();
}