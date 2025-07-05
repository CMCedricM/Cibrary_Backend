namespace Cibrary_Backend.Models;

public class CheckoutInRequest
{
    public string UserId { get; set; } = string.Empty;
    public int BookId { get; set; }
}