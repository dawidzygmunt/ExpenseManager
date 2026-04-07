using ExpensesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    // Each Db set is new table
    public DbSet<User> Users { get; set; }
    public DbSet<BlackListedToken> BlackListedTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(u => u.PasswordHash)
                .IsRequired();
        });

        modelBuilder.Entity<BlackListedToken>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Jti).IsUnique();
            entity.Property(u => u.Expiry).IsRequired();
        });
    }
}