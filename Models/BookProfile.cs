namespace Cibrary_Backend.Models
{
    public class BookProfile
    {
        public string ISBN { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public int ID { get; set; }
        public string Description { get; set;} = string.Empty;
        public int TotalAmt { get; set; } = -1; 
    }
}
