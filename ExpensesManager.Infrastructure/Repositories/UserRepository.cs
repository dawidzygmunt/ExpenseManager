using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Infrastructure.Repositories;

public class UserRepository(UserManager<User> userManager, AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<User> AddAsync(User user, string password, string role = "User")
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine,
                    result.Errors.Select(e => e.Description)));
            }

            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine,
                    roleResult.Errors.Select(e => e.Description)));
            }

            await transaction.CommitAsync();
            return user;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await userManager.GetRolesAsync(user);
    }

    public async Task AddToRoleAsync(User user, string role)
    {
        await userManager.AddToRoleAsync(user, role);
    }

    public async Task RemoveFromRoleAsync(User user, string role)
    {
        await userManager.RemoveFromRoleAsync(user, role);
    }
}