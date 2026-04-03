namespace ExpensesManager.Domain.Entities;

public class RefreshToken
{
    public required  string Token { get; set; }
    public DateTime ExpiryTime { get; set; }
}