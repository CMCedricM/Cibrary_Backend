namespace Cibrary_Backend.Models;

public enum UserRole
{
    basic, admin, founder
}

public class User : Auth0User
{
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public DateTime lastlogin { get; set; }

    public UserRole role { get; set; } = UserRole.basic;
}


public class UsersSearch
{
    public UserRole Role { get; set; }
}