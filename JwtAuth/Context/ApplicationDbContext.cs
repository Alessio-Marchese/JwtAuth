using JwtAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuth.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email)
                .IsRequired();
            entity.HasIndex(e => e.Email)
                .IsUnique();
            entity.Property(u => u.Role)
                .HasConversion<string>()
                .HasColumnType("varchar(10)");
            entity.Property(u => u.PasswordHash)
                .HasColumnType("varchar(255)");
        });
    }
}
