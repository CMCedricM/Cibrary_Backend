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


    public async Task<UsersProfile?> GetUser(UsersProfile user)
    {

        var userInfo = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == user.auth0id);

        return userInfo;
    }

    public async Task<UsersProfile> CreateNewUser(UsersProfile user)
    {
        var checkUser = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == user.auth0id);
        if (checkUser != null) return checkUser;


        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<UsersProfile?> UpdateUser(UsersProfile userProfile)
    {
        var user = await _context.Users.FirstOrDefaultAsync(b => b.auth0id == userProfile.auth0id);
        if (user != null)
        {
            var properties = typeof(UsersProfile).GetProperties();
            foreach (var prop in properties)
            {
                if (prop.Name != "id")
                {
                    var newValue = prop.GetValue(userProfile);
                    prop.SetValue(user, newValue);
                }
            }
        }
        else return null ; //return error 

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<int> RemoveUser(UsersProfile userProfile)
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