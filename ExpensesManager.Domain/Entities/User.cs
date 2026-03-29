namespace ExpensesManager.Domain.Entities;

public enum UserRole

{
  User = 1,
  Admin = 2
}

public class User
{
  public Guid Id { get; set; }

  public string Email { get; set; } = default!;

  public string PasswordHash { get; set; } = default!;

  public string FirstName { get; set; } = default!;

  public string LastName { get; set; } = default!;

  public UserRole Role { get; private set; }

  public bool IsActive { get; private set; }

}