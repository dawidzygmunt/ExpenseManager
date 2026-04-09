using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Infrastructure.Repositories;

public class UserRepository(UserManager<User> userManager): IUserRepository 
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<User> AddAsync(User user,  string password)
    {
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
        return user;
    }

    public async Task<bool>  CheckPasswordAsync(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }
}