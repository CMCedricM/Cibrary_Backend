
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
        private readonly string authId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Debug Endpoints

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

        [Authorize]
        [HttpGet("debugClaims")]
        public IActionResult DebugClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }



        // Real Endpoints Below //

        [HttpGet]
        public async Task<ActionResult<UserProfile>> GetUserInfo(UserProfile user)
        {
            var auth0User = User.FindFirst(authId)?.Value;
            if (string.IsNullOrEmpty(auth0User))
            {
                Console.WriteLine(User.Claims);
                return Unauthorized("User ID not allowed!");
            }
            else if (auth0User != user.auth0id)
            {
                return Unauthorized("Improper auth0user!");
            }
            var userInfo = await _context.GetUser(user);
            if (userInfo == null) return NotFound("User Not Found!");

            return userInfo;
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
        public async Task<ActionResult<UserProfile>> UpdateProfile(UserProfile user)
        {
            var auth0User = User.FindFirst("authId")?.Value;
            if (string.IsNullOrEmpty(auth0User) || user.auth0id == auth0User)
            {
                return Unauthorized("Invalid User!");
            }

            if (ModelState.IsValid)
            {
                int success = await _context.UpdateUser(user);
                if (success != -1) return Ok(user);

                return BadRequest("Unable to update");
            }
            else { return BadRequest(ModelState); }

        }

        [HttpDelete("removeUser")]
        [Authorize]
        public async Task<ActionResult> RemoveProfile(UserProfile user)
        {
            var auth0User = User.FindFirst("authId")?.Value;
            if (string.IsNullOrEmpty(auth0User) || user.auth0id == auth0User)
            {
                return Unauthorized("Invalid User!");
            }

            if (ModelState.IsValid)
            {
                int success = await _context.RemoveUser(user);
                if (success != -1) return Ok(user);

                return BadRequest("Unable to delete user!");
            }
            else { return BadRequest(ModelState); }

        }



    }
}
