namespace Cibrary_Backend.Models;

public class CheckInResponse
{
    public int CriculationRecordId { get; set; }
    public string UserAuth0Id { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public BookCopy? BookCopy { get; set; }
    public DateTime ReturnedDate { get; set; }
    public DateTime CurrentDate { get; set; }
}