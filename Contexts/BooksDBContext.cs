using Cibrary_Backend.Models;
using Cibrary_Backend.Enums;
using Microsoft.EntityFrameworkCore;


namespace Cibrary_Backend.Contexts;


public class BooksDBContext : DbContext
{
    public DbSet<BookProfile> Books { get; set; }

    public BooksDBContext(DbContextOptions<BooksDBContext> options)
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
        modelBuilder.Entity<BookProfile>().ToTable("books");
        modelBuilder.HasPostgresEnum<Status>();
    }




}