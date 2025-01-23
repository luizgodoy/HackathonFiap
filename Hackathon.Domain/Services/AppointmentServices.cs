using Hackathon.Core.Models;
using Hackathon.Domain.Interfaces;

namespace Hackathon.Domain.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        public Task Create(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Appointment>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Appointment> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
