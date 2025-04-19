using Cibrary_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cibrary_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
     

        private readonly BookProfile[] books =
        {
            new BookProfile
            {
                BookTitle = "I am book",
                ISBN = "217316328",
                ID = 1,
                Description = "Description",
                TotalAmt = 0,
            },
            new BookProfile
            {
                BookTitle = "I am book 2",
                ISBN = "217316328 - 2",
                ID = 80,
                Description = "Description",
                TotalAmt = 20,
            }
        };

        [HttpGet]
        public ActionResult<BookProfile[]> GetBooks()
        {
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(int id) {

            var book = books.FirstOrDefault(b => b.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        
    }
}
