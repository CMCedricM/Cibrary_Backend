using Cibrary_Backend.Models;
using Cibrary_Backend.Repository;

namespace Cibrary_Backend.Services;
public class UsersServices
{
    private readonly UsersRepository _respository;

    public UsersServices(UsersRepository repository)
    {
        _respository = repository;

    }

    public async Task<UsersProfile?> CreateUserAsync(UsersProfile newUser)
    {
        var user = await _respository.CreateNewUser(newUser);
        if (user != null)
        {
            //Some business logic
        }
        return user;
    }

    public async Task<UsersProfile?> GetUserAsync(string userId)
    {
        var user = await _respository.GetUser(userId);

        return user;
    }

    public async Task<UsersProfile?> UpdateUserAsync(UsersProfile aUser)
    {
        var profileData = await _respository.UpdateUser(aUser);

        return profileData;
    }

    public async Task<int> RemoveUserAsync(UsersProfile aUser)
    {
        int status = await _respository.RemoveUser(aUser);

        return status;
    }
}