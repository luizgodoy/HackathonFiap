using Hackathon.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentServices _AppointmentService;

        public AppointmentController(IAppointmentServices appointmentService)
        {
            _AppointmentService = appointmentService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("list-all")]
        public async Task<IActionResult> Login()
        {
            try
            {
                return Ok(await _AppointmentService.GetAll());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}