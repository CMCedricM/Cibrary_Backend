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

        public CirculationController(UsersServices userContext, CirculationServices circulationService)
        {
            _userService = userContext; _circulationService = circulationService;
        }
    }
}