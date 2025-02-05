using AutoMapper;
using Hackathon.Contract.Contracts;
using Hackathon.Core.DTO;
using Hackathon.Core.Models;
using Hackathon.Domain.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentDto appointmentDto)
        {
            await _appointmentService.Create(_mapper.Map<Appointment>(appointmentDto));
            return Ok(appointmentDto);
        }

        [HttpGet]
        [Authorize(Roles = "Doctor, Patient")]
        [Route("retrieve-appointment/{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                return Ok(_mapper.Map<AppointmentDto>(await _appointmentService.GetById(id)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Doctor, Patient")]
        [Route("list-appointment")]
        public async Task<IActionResult> GetAllAppointments()
        {
            try
            {                
                return Ok(_mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentService.GetAll()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Doctor, Patient")]
        [Route("list-appointment/{doctorId:guid}")]
        public async Task<IActionResult> GetByDoctorId([FromRoute] Guid doctorId)
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<AppointmentDto>>(await _appointmentService.GetAll(doctorId)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPatch]
        [Route("update-appointment")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<IActionResult> UpdateAppointment([FromBody] AppointmentDto appointment)
        {
            try
            {
                var model = await _appointmentService.GetById(appointment.Id);
                if (model is null) return NotFound();
                
                var updateAppointmentMessage = _mapper.Map<EditAppointmentMessage>(appointment);
                await _eventBus.Publish(updateAppointmentMessage, context => context.SetRoutingKey("update.appointment"));
            }
            catch (Exception e)
            {
                return (BadRequest(new { Message = e.Message }));
            }
            return Ok(appointment);
        }

        [HttpPatch]
        [Route("cancel-appointment")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<IActionResult> CancelAppointment([FromBody] AppointmentCancelDto cancelAppointment)
        {
            try
            {
                var model = await _appointmentService.GetById(cancelAppointment.Id);
                if (model is null) return NotFound();
                var patientId = model.PatientId;

                model.Title = cancelAppointment.Title;
                model.Description = cancelAppointment.Description;                
                model.PatientId = null;

                await _appointmentService.Cancel(model, patientId.Value);
            }
            catch (Exception e)
            {
                return (BadRequest(new { Message = e.Message }));
            }
            return Ok();
        }

        [HttpDelete]
        [Route("delete-appointment/{id:guid}")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<IActionResult> DeleteAppointment([FromRoute] Guid id)
        {
            await _appointmentService.Delete(id);
            return Ok();
        }
    }
}