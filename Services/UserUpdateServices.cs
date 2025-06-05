using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

namespace Cibrary_Backend.Services;

public class UserUpdateAuth0Services
{
    private readonly IManagementApiClient _managementApiClient;

    public UserUpdateAuth0Services(IManagementApiClient managementApiClient)
    {
        _managementApiClient = managementApiClient;
    }

    public async Task<int> UpdateUserFullNameAsync(string userId, string newFullName)
    {
        var newName = new UserUpdateRequest
        {
            FullName = newFullName
        };
        try
        {
            await _managementApiClient.Users.UpdateAsync(userId, newName);
        }
        catch (Exception e) { Console.WriteLine(e); }
        

        return 1;
    }
}