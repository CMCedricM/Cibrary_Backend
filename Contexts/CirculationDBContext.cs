

using Cibrary_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Cibrary_Backend.Contexts
{
    public class CirculationDBContext : DbContext
    {
        public DbSet<Circulation> Circulation { get; set; }

        public CirculationDBContext(DbContextOptions<CirculationDBContext> options) : base(options) { }

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
            modelBuilder.Entity<Circulation>().ToTable("circulation");
            modelBuilder.Entity<Circulation>().Property(r => r.BookStatus);

            // Mapping of dates
            modelBuilder.Entity<Circulation>().Property(r => r.CheckoutDate).HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            modelBuilder.Entity<Circulation>().Property(r => r.ReturnDate).HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            modelBuilder.Entity<Circulation>().Property(r => r.DueDate).HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        }
    }
}