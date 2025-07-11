using Cibrary_Backend.Models;
using Microsoft.EntityFrameworkCore;


namespace Cibrary_Backend.Contexts;

public class BookCopyDBContext : DbContext
{
    public DbSet<BookCopy> BookCopy { get; set; }

    public BookCopyDBContext(DbContextOptions<BookCopyDBContext> options) : base(options) { }

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
        modelBuilder.HasPostgresEnum<BookStatus>();
        modelBuilder.Entity<BookCopy>().ToTable("bookscopy");
        modelBuilder.Entity<BookCopy>().Property(r => r.Status);
    }
}