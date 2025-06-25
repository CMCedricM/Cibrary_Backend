using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Cibrary_Backend.Models;

namespace Cibrary_Backend.Services;

public class UserUpdateAuth0Services
{
    private readonly IManagementApiClient _managementApiClient;

    public UserUpdateAuth0Services(IManagementApiClient managementApiClient)
    {
        _managementApiClient = managementApiClient;
    }

    public async Task<Auth0.ManagementApi.Models.User?> UpdateUserFullNameAsync(string userId, UsersProfile userInfo)
    {
        var currentUser = await _managementApiClient.Users.GetAsync(userId);
        if (currentUser == null) return null;

        try
        {
            // Always check fro email update 
            var userUpdate = new UserUpdateRequest
            {
                Email = userInfo.email,
                FullName = userInfo.name,
                EmailVerified = false,
            };

            var user = await _managementApiClient.Users.UpdateAsync(userId, userUpdate);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;


    }
}