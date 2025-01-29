using Hackathon.Core.Models;
using Hackathon.Data.Context;
using Hackathon.Data.Interfaces;

namespace Hackathon.Data.Repository
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HackathonDbContext dbContext) : base(dbContext) { }
    }
}
