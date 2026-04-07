namespace ExpensesManager.Domain.Entities;

public class BlackListedToken
{
    public Guid Id { get; set; }
    public required string Jti { get; set; }
    public DateTime Expiry { get; set; }
}