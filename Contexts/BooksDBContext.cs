using Cibrary_Backend.Models;
using Cibrary_Backend.Enums;
using Microsoft.EntityFrameworkCore;


namespace Cibrary_Backend.Contexts;


public class BookDBContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public BookDBContext(DbContextOptions<BookDBContext> options)
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
        modelBuilder.Entity<Book>().ToTable("books");
        modelBuilder.HasPostgresEnum<Status>();
    }




}