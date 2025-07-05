using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cibrary_Backend.Models;

public enum BookStatus
{
    returned,
    checked_out,
    overdue,
    pending
}
[Table("circulation")]
public class Circulation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Required]
    [Column("bookcopy_id")]
    public int BookCopyId { get; set; }

    [ForeignKey("BookCopyId")]
    public BookCopy Book { get; set; } = null!;

    [Column("checkout_date")]
    public DateTime CheckoutDate { get; set; }

    [Column("due_date")]
    public DateTime DueDate { get; set; }

    [Column("return_date")]
    public DateTime ReturnDate { get; set; }

    [Column("book_status", TypeName = "book_status")]
    public BookStatus BookStatus { get; set; } = BookStatus.pending;


}