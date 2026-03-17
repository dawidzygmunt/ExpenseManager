namespace ExpensesManager.Domain.Entities;

public enum UserRole

{
  User = 1,
  Admin = 2
}

public class User
{
  public Guid Id { get; private set; }

  public string Email { get; private set; } = default!;

  public string PasswordHash { get; private set; } = default!;

  public string FirstName { get; private set; } = default!;

  public string LastName { get; private set; } = default!;

  public UserRole Role { get; private set; }

  public bool IsActive { get; private set; }

}