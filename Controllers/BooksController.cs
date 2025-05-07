using Cibrary_Backend.Contexts;
using Cibrary_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cibrary_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {

            int cnt = await _context.FetchBookCount();
            return Ok(cnt);
        }


        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetBook(int id)
        {

            var book = books.FirstOrDefault(b => b.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }



    }
}
