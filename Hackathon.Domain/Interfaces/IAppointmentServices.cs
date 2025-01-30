using Hackathon.Core.Models;

namespace Hackathon.Domain.Interfaces
{
    public interface IAppointmentServices
    {
        Task Create(Appointment appointment);
        Task<Appointment> GetById(Guid id);
        Task<IEnumerable<Appointment>> GetAll();
        Task<IEnumerable<Appointment>> GetAll(Guid? doctorId);
        Task Update(Appointment appointment);
        Task Delete(Guid id);
        Task Notify(Appointment appointment);
    }
}