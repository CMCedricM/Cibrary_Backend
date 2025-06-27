
using Cibrary_Backend.Services;
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
        private readonly UserUpdateAuth0Services _userUpdateService;
        private readonly string authId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public UsersController(UsersServices context, UserUpdateAuth0Services userContext)
        {
            _userService = context;
            _userUpdateService = userContext;
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
        public ActionResult<Auth0UserProfile> hello()
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

        [HttpGet("{auth0id}")]
        public async Task<ActionResult<UsersProfile>> GetUserInfo([FromRoute]string auth0id)
        {
            var auth0User = User.FindFirst(authId)?.Value;
            if (string.IsNullOrEmpty(auth0User) || auth0User != auth0id) return Unauthorized();

            var userInfo = await _userService.GetUserAsync(auth0id);
            if (userInfo == null) return NotFound("User Not Found!");

            return Ok(userInfo);
        }


        [HttpPost("createUser")]
        [Authorize]
        public async Task<ActionResult<UsersProfile>> CreateProfile(UsersProfile profile)
        {
            if (!ModelState.IsValid) return BadRequest();

            var checkUser = await _userService.GetUserAsync(profile.auth0id);
            if (checkUser != null) return Conflict(new { message = "User already exists!" });

            await _userService.CreateUserAsync(profile);

            return Ok(profile);
        }


        [HttpPost("updateUser")]
        [Authorize]
        public async Task<ActionResult<UsersProfile>> UpdateProfile(UsersProfile user)
        {
            var auth0User = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (string.IsNullOrEmpty(auth0User) || user.auth0id != auth0User) return Unauthorized();

            if (ModelState.IsValid)
            {
                var updatedData = await _userService.UpdateUserAsync(user);
                // Submit the update to auth0
                await _userUpdateService.UpdateUserFullNameAsync(auth0User, user);
                if (updatedData != null) return Ok(user);
            }

            return BadRequest(ModelState);

        }

        [HttpDelete("removeUser")]
        [Authorize]
        public async Task<ActionResult> RemoveProfile(UsersProfile user)
        {
            var auth0User = User.FindFirst(authId)?.Value;
            if (string.IsNullOrEmpty(auth0User) ||user.auth0id != auth0User) return Unauthorized();


            // Also check role is ok
            var userRole = await _userService.GetUserAsync(auth0User);
            if (userRole == null || userRole.role != UserRole.founder) return Unauthorized();

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
