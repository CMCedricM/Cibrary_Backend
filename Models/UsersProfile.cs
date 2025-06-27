namespace Cibrary_Backend.Models;

public enum UserRole
{
    basic, admin, founder
}

public class UsersProfile : Auth0UserProfile
{
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public DateTime lastlogin { get; set; }

    public UserRole role { get; set; } = UserRole.basic;
}