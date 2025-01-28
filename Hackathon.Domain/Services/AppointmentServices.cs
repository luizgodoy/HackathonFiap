using Hackathon.Core.Models;
using Hackathon.Data.Interfaces;
using Hackathon.Domain.Interfaces;
using Hackathon.Domain.Validators;

namespace Hackathon.Domain.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentServices(IAppointmentRepository appointmentRepository)
        {            
            _appointmentRepository = appointmentRepository;
        }

        public async Task Create(Appointment appointment)
        {
            var appointmentValidator = new AppointmentValidator();
            var result = appointmentValidator.Validate(appointment);

            if (!result.IsValid)
                throw new Exception(result.Errors.FirstOrDefault().ErrorMessage);

            await _appointmentRepository.Create(appointment);
        }

        public async Task Delete(Guid id)
        {
            await _appointmentRepository.Delete(id);
        }

        public async Task<IEnumerable<Appointment>> GetAll(Guid? doctorId)
        {
            var appointments = await _appointmentRepository.GetAll();

            if(doctorId != null)
                return appointments.Where(a => a.DoctorId.Equals(doctorId));

            return appointments;
        }

        public async Task<Appointment> GetById(Guid id)
        {
            return await _appointmentRepository.GetById(id);
        }

        public async Task Update(Appointment appointment)
        {
            await _appointmentRepository.Update(appointment);
        }
    }
}
