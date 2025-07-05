using Cibrary_Backend.Models;
using Cibrary_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cibrary_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CirculationController : ControllerBase
    {
        private readonly UsersServices _userService;
        private readonly CirculationServices _circulationService;

        private readonly string authId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public CirculationController(UsersServices userContext, CirculationServices circulationService)
        {
            _userService = userContext; _circulationService = circulationService;
        }

        [HttpGet("bookCopy/{id}")]
        [Authorize]
        public async Task<ActionResult<BookCopy>> GetBookCopyById([FromRoute] int id)
        {
            var auth0User = User.FindFirst(authId)?.Value;
            if (string.IsNullOrEmpty(auth0User)) return Unauthorized();
            var permission = await _userService.GetUserAsync(auth0User);
            if (permission == null || permission.role != UserRole.admin) return Unauthorized();

            var bookData = await _circulationService.GetBookCopyByIdAsync(id);
            if (bookData != null)
            {
                return Ok(bookData);
            }

            return NotFound("Could Not Locate Book");

        }

        [HttpPost("checkoutBook")]
        [Authorize]
        public async Task<ActionResult<Circulation>> Checkout([FromBody] CheckoutInRequest body)
        {
            // First we need to check the permissions of the account running checkout
            var auth0User = User.FindFirst(authId)?.Value;
            if (string.IsNullOrEmpty(auth0User)) return Unauthorized();
            // Now we need to check their permissions
            var permission = await _userService.GetUserAsync(auth0User);
            if (permission == null || permission.role != UserRole.admin) return Unauthorized();

            // Now we can check out the user
            try
            {
                Circulation book = await _circulationService.CheckoutBook(body.BookId, body.UserId);
                return Ok(book);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }


    }
}