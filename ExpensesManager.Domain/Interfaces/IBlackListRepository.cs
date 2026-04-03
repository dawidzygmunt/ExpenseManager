using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Domain.Interfaces;

public interface IBlackListRepository
{
    Task AddAsync(BlackListedToken token);
    Task<bool> IsBlacklistedAsync(string jti);
}