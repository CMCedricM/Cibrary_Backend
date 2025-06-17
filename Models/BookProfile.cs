using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cibrary_Backend.Models
{
    public class BookProfile
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        [Column("isbn")]
        [Required]
        public string ISBN { get; set; } = string.Empty;
        [Column("title")]
        public string Title { get; set; } = string.Empty;
        [Column("releasedate")]
        public DateTime? ReleaseDate { get; set; } = DateTime.Now;
        [Column("availabilitycnt")]
        public int AvailabilityCnt { get; set; } = 0;
        [Column("totalcnt")]
        public int TotalCnt { get; set; } = 0;
    }
}
