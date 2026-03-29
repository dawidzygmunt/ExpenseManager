using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;

namespace ExpensesManager.Infrastructure.Repositories;

public class MockUserRepository : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email)
    {
        var fakeUser = new User
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
            Email = "test@test.com",
            FirstName = "Jan",
            LastName = "Kowalski",
            PasswordHash = "hashed123"
        };

        if (email == fakeUser.Email)
            return Task.FromResult<User?>(fakeUser);

        return Task.FromResult<User?>(null);
    }
}