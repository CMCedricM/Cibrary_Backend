using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cibrary_Backend.Models;

public class BookCopy
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int ID { get; set; }

    [Column("uuid")]
    public Guid? UUID { get; set; }

    [Column("book_id")]
    public int Book_ID { get; set; }
}