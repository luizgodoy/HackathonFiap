using Hackathon.Core.Models;

namespace Hackathon.Domain.Interfaces;

public interface ISpecialtyService 
{
    Task Create(string specialtyName, Guid doctorId);
    
}
