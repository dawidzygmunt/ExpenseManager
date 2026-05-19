namespace ExpensesManager.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Token { get; set; }
    public DateTime ExpiryTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiryTime;
}