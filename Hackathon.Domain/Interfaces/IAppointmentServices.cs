using Hackathon.Core.Models;

namespace Hackathon.Domain.Interfaces
{
    public interface IAppointmentServices
    {
        Task Create(Appointment appointment);
        Task<Appointment> GetById(long id);
        Task<IEnumerable<Appointment>> GetAll();
        Task Update(Appointment appointment);
        Task Delete(long id);
    }
}