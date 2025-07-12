using Auth0.ManagementApi.Models.Actions;
using Cibrary_Backend.dtos;
using Cibrary_Backend.Errors;
using Cibrary_Backend.Models;
using Cibrary_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Expressions;

namespace Cibrary_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CirculationController : ControllerBase
    {
        private readonly UsersServices _userService;
        private readonly CirculationServices _circulationService;

        private readonly string AUTH_ID_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public CirculationController(UsersServices userContext, CirculationServices circulationService)
        {
            _userService = userContext; _circulationService = circulationService;
        }

        [HttpGet("bookCopy/{id}")]
        [Authorize]
        public async Task<ActionResult<BookCopy>> GetBookCopyById([FromRoute] int id)
        {
            var auth0User = User.FindFirst(AUTH_ID_KEY)?.Value;
            if (string.IsNullOrEmpty(auth0User)) return Unauthorized();
            var permission = await _userService.GetUserAsync(auth0User);
            if (permission == null || permission.Role != UserRole.admin) return Unauthorized();

            var bookData = await _circulationService.GetBookCopyByIdAsync(id);
            if (bookData != null)
            {
                return Ok(bookData);
            }

            return NotFound("Could Not Locate Book");

        }
        [HttpDelete("cancelCheckout/{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveCheckout([FromRoute] int id)
        {
            var auth0User = User.FindFirst(AUTH_ID_KEY)?.Value;
            if (string.IsNullOrEmpty(auth0User)) return Unauthorized();
            var permissions = await _userService.GetUserAsync(auth0User);
            if (permissions == null || permissions.Role != UserRole.admin) return Unauthorized();

            try
            {
                var removeBook = await _circulationService.CancelCheckout(id);
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost("completeCheckout/{id}")]
        [Authorize]
        public async Task<ActionResult<Circulation?>> CompleteCheckout([FromRoute] int id)
        {
            var auth0User = User.FindFirst(AUTH_ID_KEY)?.Value;
            if (string.IsNullOrEmpty(auth0User)) return Unauthorized();
            // verify they have admin permissions
            var permissions = await _userService.GetUserAsync(auth0User);
            if (permissions == null || permissions.Role != UserRole.admin) return Unauthorized();

            var res = await _circulationService.CompleteCheckout(id);

            if (res == null) return NotFound();

            return Ok(res);
        }

        [HttpPost("checkoutBook")]
        [Authorize]
        public async Task<ActionResult<CheckoutResponse>> Checkout([FromBody] CheckoutInRequest body)
        {
            // First we need to check the permissions of the account running checkout
            var auth0User = User.FindFirst(AUTH_ID_KEY)?.Value;
            if (string.IsNullOrEmpty(auth0User)) return Unauthorized();
            // Now we need to check their permissions
            var permission = await _userService.GetUserAsync(auth0User);
            if (permission == null || permission.Role != UserRole.admin) return Unauthorized();

            // Now we can check out the user
            try
            {
                CheckoutResponse book = await _circulationService.CheckoutBook(body.BookId, body.UserId);
                return Ok(book);
            }
            catch (ConflictFound e)
            {
                Console.WriteLine(e);
                return StatusCode(e.StatusCode, new { error = e.Message });
            }
        }

        [HttpPost("checkInABook")]
        [Authorize]
        public async Task<ActionResult<CheckInResponse>> Checkin([FromBody] CheckoutInRequest body)
        {
            var auth0User = User.FindFirst(AUTH_ID_KEY)?.Value;
            if (string.IsNullOrEmpty(auth0User)) return Unauthorized();
            var permission = await _userService.GetUserAsync(auth0User);
            if (permission == null || permission.Role != UserRole.admin) return StatusCode(401, new { error = "Please verify you have permissions for this action!" });
            try
            {
                CheckInResponse res = await _circulationService.CheckinBook(body.BookId, body.UserId);
                return res;
            }
            catch (DataNotFound e)
            {
                return StatusCode(e.StatusCode, new { error = e.Message });
            }
        }



    }
}