using Cibrary_Backend.Models;
using Cibrary_Backend.Repository;

namespace Cibrary_Backend.Services;

public class UsersServices
{
    private readonly UserRepository _respository;

    public UsersServices(UserRepository repository)
    {
        _respository = repository;

    }

    public async Task<User?> CreateUserAsync(User newUser)
    {
        var user = await _respository.CreateNewUser(newUser);
        if (user != null)
        {
            //Some business logic
        }
        return user;
    }

    public async Task<User?> GetUserAsync(string userId)
    {
        var user = await _respository.GetUser(userId);

        return user;
    }

    public async Task<List<User>?> GetUsersAsync(UsersSearch query)
    {
        var userResults = await _respository.GetUsers(query);

        return userResults;
    }

    public async Task<User?> UpdateUserAsync(User aUser)
    {
        var profileData = await _respository.UpdateUser(aUser);

        return profileData;
    }

    public async Task<int> RemoveUserAsync(User aUser)
    {
        int status = await _respository.RemoveUser(aUser);

        return status;
    }
}