namespace ExpensesManager.Domain.Interfaces;

public interface IPasswordHasher
{
    bool Verify(string password, string hashedPassword);
}