using AutoMapper;
using Hackathon.Contract.Contracts;
using Hackathon.Core.DTO;
using Hackathon.Core.Models;
using Hackathon.Domain.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentServices _appointmentService;
        private readonly IMapper _mapper;
        private readonly IBus _eventBus;

        public AppointmentController(IAppointmentServices appointmentService, IMapper mapper, IBus eventBus)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        [Route("create-appointment")]
        //[Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentDto appointmentDto)
        {
            await _appointmentService.Create(_mapper.Map<Appointment>(appointmentDto));
            return Ok(appointmentDto);
        }

        [HttpGet]
        //[Authorize(Roles = "Doctor, Patient")]
        [Route("retrieve-appointment/{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _appointmentService.GetById(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        //[Authorize(Roles = "Doctor, Patient")]
        [Route("list-appointment/{docatorId:guid}")]
        public async Task<IActionResult> GetByDoctorId([FromRoute]Guid? doctorId)
        {
            try
            {
                return Ok(await _appointmentService.GetAll(doctorId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPatch]
        [Route("update-appointment")]
        //[Authorize(Roles = "Doctor, Patient")]
        public async Task<IActionResult> UpdateAppointment([FromBody] AppointmentDto appointment)
        {
            try
            {
                var contact = await _appointmentService.GetById(appointment.Id);

                if (contact is null)
                {
                    return NotFound();
                }

                var updateContactMessage = _mapper.Map<EditAppointmentMessage>(appointment);
                await _eventBus.Publish(updateContactMessage, context => context.SetRoutingKey("update.appointment"));

            }
            catch (Exception e)
            {
                return (BadRequest(new { Message = e.Message }));
            }

            return Ok(appointment);
        }

        [HttpDelete]
        [Route("delete-appointment/{id:guid}")]
        //[Authorize(Roles = "Doctor, Patient")]
        public async Task<IActionResult> DeleteAppointment([FromRoute] Guid id)
        {
            await _appointmentService.Delete(id);
            return Ok();
        }
    }
}