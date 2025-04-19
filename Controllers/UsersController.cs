using System.Threading.Tasks;
using Cibrary_Backend.Contexts;
using Cibrary_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cibrary_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        static private readonly UserProfile testProfile = new()
        {
            username = "testUser",
            email = "test@email.com",
            firstname = "FirstName"
        };


        [HttpGet]
        public ActionResult<UserProfile> GetUserInfo()
        {
            return Ok(testProfile);
        }

        [HttpGet("private")]
        [Authorize]
        public IActionResult Private()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated to see this."
            });
        }

        [HttpGet("hello")]
        [Authorize]
        public ActionResult<UserProfile> hello()
        {
            return Ok("Hello");

        }

        [HttpPost("createUser")]
        [Authorize]
        public async Task<ActionResult<UserProfile>> CreateProfile(UserProfile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _context.CreateNewUser(profile);

            return Ok(profile);
        }
        [HttpPost("updateUser")]
        [Authorize]
        public ActionResult<UserProfile> UpdateProfile(UserProfile user)
        {
            if (ModelState.IsValid) { return Ok(user); }
            else { return BadRequest(ModelState); }

        }



    }
}
