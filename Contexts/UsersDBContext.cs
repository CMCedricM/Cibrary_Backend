using Cibrary_Backend.Models;
using Cibrary_Backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cibrary_Backend.Contexts;

public class UserDBContext : DbContext
{

    public DbSet<User> Users { get; set; }

    public UserDBContext(DbContextOptions<UserDBContext> options)
       : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL_DOTNET");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>()
      .Property(u => u.Role)
      .HasColumnType("user_status");
    }


}