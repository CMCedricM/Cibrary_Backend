using Cibrary_Backend.Contexts;
using Cibrary_Backend.Models;
using Microsoft.EntityFrameworkCore;


namespace Cibrary_Backend.Repository;

public class UsersRepository
{
    private readonly UsersDBContext _context;

    public UsersRepository(UsersDBContext context)
    {
        _context = context;
    }


    public async Task<UserProfile?> GetUser(UserProfile user)
    {

        var userInfo = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == user.auth0id);

        return userInfo;
    }

    public async Task<UserProfile> CreateNewUser(UserProfile user)
    {
        var checkUser = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == user.auth0id);
        if (checkUser != null) return checkUser;

        if (user.lastlogin == null)
        {
            var time = DateTime.UtcNow.ToUniversalTime();
            user.lastlogin = time;
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<int> UpdateUser(UserProfile userProfile)
    {
        var user = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == userProfile.auth0id);
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

        await _context.SaveChangesAsync();

        return 0;
    }

    public async Task<int> RemoveUser(UserProfile userProfile)
    {
        var user = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == userProfile.auth0id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        else return -1;

        return 0;
    }

}