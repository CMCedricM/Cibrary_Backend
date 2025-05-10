
using Cibrary_Backend.Services;
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

        private readonly UsersServices _userService;
        private readonly string authId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public UsersController(UsersServices context)
        {
            _userService = context;
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
            if (string.IsNullOrEmpty(auth0User) || auth0User != user.auth0id) return Unauthorized();

            var userInfo = await _userService.GetUserAsync(user);
            if (userInfo == null) return NotFound("User Not Found!");

            return Ok(userInfo);
        }


        [HttpPost("createUser")]
        [Authorize]
        public async Task<ActionResult<UserProfile>> CreateProfile(UserProfile profile)
        {
            if (!ModelState.IsValid) return BadRequest();

            var checkUser = await _userService.GetUserAsync(profile);
            if (checkUser != null) return Conflict(new { message = "User already exists!" });

            await _userService.CreateUserAsync(profile);

            return Ok(profile);
        }


        [HttpPost("updateUser")]
        [Authorize]
        public async Task<ActionResult<UserProfile>> UpdateProfile(UserProfile user)
        {
            var auth0User = User.FindFirst("authId")?.Value;
            if (string.IsNullOrEmpty(auth0User) || user.auth0id == auth0User) return Unauthorized();

            if (ModelState.IsValid)
            {
                int success = await _userService.UpdateUserAsync(user);
                if (success != -1) return Ok(user);
            }

            return BadRequest(ModelState);

        }

        [HttpDelete("removeUser")]
        [Authorize]
        public async Task<ActionResult> RemoveProfile(UserProfile user)
        {
            var auth0User = User.FindFirst("authId")?.Value;
            if (string.IsNullOrEmpty(auth0User) || user.auth0id == auth0User) return Unauthorized();

            if (ModelState.IsValid)
            {
                int success = await _userService.RemoveUserAsync(user);
                if (success != -1) return Ok(user);
                return NotFound("Could not find user!");

            }

            return BadRequest();

        }



    }
}
