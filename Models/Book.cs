using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cibrary_Backend.Models
{
    [Table("books")]
    public class Book
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int ID { get; set; }

        [Column("uuid")]
        public Guid? Uuid { get; set; }
        [Column("isbn")]
        [Required]
        public string Isbn { get; set; } = string.Empty;
        [Column("title")]
        public string Title { get; set; } = string.Empty;
        [Column("releasedate")]
        public DateTime? ReleaseDate { get; set; } = DateTime.UtcNow;

        [Column("created_at")]
        public DateTime Created_At { get; set; } = DateTime.UtcNow;

        [Column("availabilitycnt")]
        public int AvailabilityCnt { get; set; } = 0;
        [Column("totalcnt")]
        public int TotalCnt { get; set; } = 0;
        [Column("description")]
        public string Description { get; set; } = String.Empty;
    }

    public class BookSearch
    {
        public string Isbn { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
    }
}
