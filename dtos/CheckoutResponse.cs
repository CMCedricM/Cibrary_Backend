using Cibrary_Backend.Models;

namespace Cibrary_Backend.dtos;

public class CheckoutResponse
{
    public int Id { get; set; }
    public string UserAuth0Id { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public BookCopy? BookCopy { get; set; }
}