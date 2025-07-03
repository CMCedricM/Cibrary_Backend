using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cibrary_Backend.Models
{
    [Table("users")]
    public class Auth0User
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string auth0id { get; set; } = string.Empty;

        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string phone_number { get; set; } = string.Empty;

    }
}
