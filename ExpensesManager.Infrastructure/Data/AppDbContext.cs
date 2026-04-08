using ExpensesManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ExpensesManager.Infrastructure.Data;

public class AppDbContext : IdentityDbContext <User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    // Each Db set is new table
    public DbSet<BlackListedToken> BlackListedTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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
    }
}