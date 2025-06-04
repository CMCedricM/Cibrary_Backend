using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

namespace Cibrary_Backend.Services;

public class UserUpdateServices
{
    private readonly IManagementApiClient _managementApiClient;

    public UserUpdateServices(IManagementApiClient managementApiClient)
    {
        _managementApiClient = managementApiClient;
    }
    
      public async Task<User> UpdateUserFullNameAsync(string userId, string newFullName)
    {
        var newName = new UserUpdateRequest{
           FullName = "Cedric Men"
        };


        return await _managementApiClient.Users.UpdateAsync(userId, newName);
    }
}