using ExpensesManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Each Db set is new table
    public DbSet<BlackListedToken> BlackListedTokens { get; set; }
    public DbSet<Expense> Expenses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "admin-stamp"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "user-stamp"
            }
        );

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<BlackListedToken>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Jti).IsUnique();
            entity.Property(u => u.Expiry).IsRequired();
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.GoalDeductionPercentage).HasColumnType("decimal(5,2)");
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.Currency).HasConversion<string>();
            entity.Property(e => e.PaymentMethod).HasConversion<string>();
            entity.HasIndex(e => e.WorkspaceId);
            entity.HasIndex(e => new { e.UserId, e.WorkspaceId });
        });
    }
}