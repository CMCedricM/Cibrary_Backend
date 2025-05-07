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

    public async Task<UserProfile?> GetUser(UserProfile user)
    {

        var userInfo = await Users.FirstOrDefaultAsync(b => b.auth0id == user.auth0id);

        return userInfo;
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
        var user = await Users.FirstOrDefaultAsync(b => b.auth0id == userProfile.auth0id);
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

    public async Task<int> RemoveUser(UserProfile userProfile)
    {
        var user = await Users.FirstOrDefaultAsync(b => b.auth0id == userProfile.auth0id);
        if (user != null)
        {
            Users.Remove(user);
            await SaveChangesAsync();
        }
        else return -1;

        return 0;
    }

    public async Task<int> FetchBookCount()
    {
        int booksCnt = await BookProfiles.CountAsync();
        return booksCnt;
    }

}
