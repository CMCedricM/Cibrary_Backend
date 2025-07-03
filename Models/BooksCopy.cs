using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cibrary_Backend.Models;

[Table("bookscopy")]
public class BookCopy
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int ID { get; set; }

    [Column("uuid")]
    public Guid? UUID { get; set; }

    [Required]
    [Column("book_id")]
    public int BookId { get; set; }

    [ForeignKey("BookId")]
    public Book Book { get; set; } = null!;
}