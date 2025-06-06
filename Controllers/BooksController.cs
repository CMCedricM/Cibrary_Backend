using Cibrary_Backend.Services;
using Cibrary_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cibrary_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {

        private readonly BooksServices _context;
        public BooksController(BooksServices context)
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

            int cnt = await _context.GetBookCountAsync();
            return Ok(cnt);
        }




    }
}
