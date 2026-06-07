using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Api.UnitTest.builders;

public class UserBuilder
{
    private string _email = "test@test.com";
    private string _firstName = "John";
    private string _lastName = "Doe";
    private string _passwordHash = "hashedPassword";
    private string _userName = "test@test.com";

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        _userName = email;
        return this;
    }

    public UserBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public UserBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public UserBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public User Build()
    {
        return new User
        {
            Email = _email,
            UserName = _userName,
            FirstName = _firstName,
            LastName = _lastName,
            PasswordHash = _passwordHash
        };
    }
}