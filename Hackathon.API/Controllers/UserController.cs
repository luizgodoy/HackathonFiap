using Hackathon.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userServices)
        {
            _userService = userServices;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            try
            {
                return Ok(await _userService.GetAll());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}