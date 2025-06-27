using Cibrary_Backend.Models;
using Cibrary_Backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cibrary_Backend.Contexts;

public class UsersDBContext : DbContext
{

    public DbSet<UsersProfile> Users { get; set; }

    public UsersDBContext(DbContextOptions<UsersDBContext> options)
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
        modelBuilder.HasPostgresEnum<UserRole>();
        modelBuilder.Entity<UsersProfile>().ToTable("users");
        modelBuilder.Entity<UsersProfile>().Property(r => r.role);
    }


}