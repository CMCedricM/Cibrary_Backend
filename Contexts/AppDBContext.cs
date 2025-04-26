using System;
using Cibrary_Backend.Models;
using Cibrary_Backend.Enums;
using Microsoft.EntityFrameworkCore;


namespace Cibrary_Backend.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserProfile> Users { get; set; }
    public DbSet<BookProfile> BookProfiles { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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
        modelBuilder.Entity<UserProfile>().ToTable("users");
        modelBuilder.Entity<BookProfile>().ToTable("books");
        modelBuilder.HasPostgresEnum<Status>();
    }

    public async Task<UserProfile> CreateNewUser(UserProfile user)
    {
        if (user.lastlogin == null)
        {
            var time = DateTime.UtcNow.ToUniversalTime();
            user.lastlogin = time;
        }
        Users.Add(user);
        await SaveChangesAsync();
        return user;
    }

    public async Task<int> UpdateUser(UserProfile userProfile)
    {
        var user = await Users.SingleAsync(b => b.auth0id == userProfile.auth0id);
        if (user != null)
        {
            var properties = typeof(UserProfile).GetProperties();
            foreach (var prop in properties)
            {
                if (prop.Name != "id")
                {
                    var newValue = prop.GetValue(userProfile);
                    prop.SetValue(user, newValue);
                }
            }
        }
        else return -1; //return error 
        await SaveChangesAsync();

        return 0;
    }

}
