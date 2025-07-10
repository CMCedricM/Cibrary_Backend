using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cibrary_Backend.Models;

// For a book copy this should be the source of truth
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

    [Required]
    [Column("status", TypeName = "book_status")]
    public BookStatus Status { get; set; } = BookStatus.returned;

    [ForeignKey("BookId")]
    public Book Book { get; set; } = null!;
}