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

    public async Task<int> UpdateUserFullNameAsync(string userId, UsersProfile userInfo)
    {
        Console.WriteLine(userInfo);
        var userUpdate = new UserUpdateRequest
        {
            FullName = userInfo.name
        };
        try
        {
            await _managementApiClient.Users.UpdateAsync(userId, userUpdate);
        }
        catch (Exception e) { Console.WriteLine(e); }
        

        return 1;
    }
}