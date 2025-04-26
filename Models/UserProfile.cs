using System.ComponentModel.DataAnnotations;

namespace Cibrary_Backend.Models
{
    public class UserProfile
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string auth0id { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public string firstname { get; set; } = string.Empty;
        public string? lastname { get; set; }
        public DateTime? lastlogin { get; set; }


    }
}
