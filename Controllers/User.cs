using Cibrary_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cibrary_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class User : Controller
    {

        static private readonly UserProfile testProfile = new()
        {
            UserName = "testUser",
            Email = "test@email.com",
            FirstName = "FirstName"
        };

        public IActionResult Index()
        {
            return View();
        }

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

        [HttpPost]
        [Authorize]
        public IActionResult UpdateProfile(UserProfile user)
        {

            return Ok(user); 
        }
       


    }
}
