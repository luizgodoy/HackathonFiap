using Hackathon.Core.Models;

namespace Hackathon.Domain.Interfaces;

public interface ISpecialtyService 
{
    Task Create(string specialtyName, Guid doctorId);    
    Task<IEnumerable<Specialty>> GetAll();
    Task<IEnumerable<Specialty>> GetAll(Guid? doctorId);
    Task Update(Specialty specialty);
    Task Cancel(Specialty specialty, Guid patientId);
    Task Delete(Guid id);
}
