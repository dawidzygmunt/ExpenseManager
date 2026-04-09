using Microsoft.AspNetCore.Identity;

namespace ExpensesManager.Domain.Entities;

public class User: IdentityUser<Guid>
{
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public bool IsActive { get; private set; }
}