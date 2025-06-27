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
    public UsersProfile User { get; set; } = null!;

    [Required]
    [Column("book_id")]
    public int BookId { get; set; }

    [ForeignKey("BookId")]
    public BookProfile Book { get; set; } = null!;

    [Column("checkout_date")]
    public DateTime? CheckoutDate { get; set; }

    [Column("return_date")]
    public DateTime? ReturnDate { get; set; }

    [Column("book_status", TypeName = "book_status")]
    public BookStatus BookStatus { get; set; } = BookStatus.pending;


}