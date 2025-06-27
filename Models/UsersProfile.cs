namespace Cibrary_Backend.Models;

public enum UserStatus
{
    basic, admin, founder
}

public class UsersProfile : Auth0UserProfile
{
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public DateTime lastlogin { get; set; }

    public UserStatus role { get; set; } = UserStatus.basic;
}