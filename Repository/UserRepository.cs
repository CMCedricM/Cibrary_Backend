using Cibrary_Backend.Contexts;
using Cibrary_Backend.Models;
using Microsoft.EntityFrameworkCore;


namespace Cibrary_Backend.Repository;

public class UserRepository
{
    private readonly UserDBContext _context;

    public UserRepository(UserDBContext context)
    {
        _context = context;
    }


    public async Task<User?> GetUser(string userId)
    {

        var userInfo = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == userId);

        return userInfo;
    }

    public async Task<List<User>?> GetUsers(UsersSearch query)
    {
        if (!string.IsNullOrWhiteSpace(query.Role.ToString()))
        {
            var userData = await _context.Users.Where(p => p.Role == query.Role).ToListAsync();
            return userData;
        }

        return null;
    }
    public async Task<User> CreateNewUser(User user)
    {
        var checkUser = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == user.auth0id);
        if (checkUser != null) return checkUser;


        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> UpdateUser(User userProfile)
    {
        var user = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == userProfile.auth0id);
        if (user != null)
        {
            var properties = typeof(User).GetProperties();
            foreach (var prop in properties)
            {
                if (prop.Name != "id")
                {
                    var newValue = prop.GetValue(userProfile);
                    prop.SetValue(user, newValue);
                }
            }
        }
        else return null; //return error 

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<int> RemoveUser(User userProfile)
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