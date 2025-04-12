using Cibrary_Backend.Models;
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

        
       
    }
}
