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
                Title = "I am book",
                ISBN = "217316328",
                id = 1,
                TotalCnt = 0,
            },
             new BookProfile
            {
                Title = "I am book",
                ISBN = "217316328",
                id = 2,
                TotalCnt = 80,
            },
        };

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {

            int cnt = await _context.GetBookCountAsync();
            return Ok(cnt);
        }




    }
}
