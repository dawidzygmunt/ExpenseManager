using ExpensesManager.Domain.Interfaces;

namespace ExpensesManager.Infrastructure.Services;



public class MockPasswordHasher: IPasswordHasher
{
    public bool Verify(string hashedPassword, string password)
    {
        return true;
    }
}