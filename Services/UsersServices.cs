using Cibrary_Backend.Models;
using Cibrary_Backend.Repository;

public class UsersServices
{
    private readonly UsersRepository _respository;

    public UsersServices(UsersRepository repository)
    {
        _respository = repository;

    }

    public async Task<UserProfile?> CreateUserAsync(UserProfile newUser)
    {
        var user = await _respository.CreateNewUser(newUser);
        if (user != null)
        {
            //Some business logic
        }
        return user;
    }

    public async Task<UserProfile?> GetUserAsync(UserProfile aUser)
    {
        var user = await _respository.GetUser(aUser);

        return user;
    }

    public async Task<int> UpdateUserAsync(UserProfile aUser)
    {
        int status = await _respository.UpdateUser(aUser);

        return status;
    }

    public async Task<int> RemoveUserAsync(UserProfile aUser)
    {
        int status = await _respository.RemoveUser(aUser);

        return status;
    }
}