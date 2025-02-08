using Hackathon.Core.Models;

namespace Hackathon.Data.Interfaces;

public interface ISpecialtyRepository : IRepository<Specialty>
{
    Task<Specialty?> CheckExistingSpecialty(Specialty specialty);
}
