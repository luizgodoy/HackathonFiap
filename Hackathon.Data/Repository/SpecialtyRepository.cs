using Hackathon.Core.Models;
using Hackathon.Data.Context;
using Hackathon.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Data.Repository
{
    public class SpecialtyRepository : Repository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepository(HackathonDbContext db) : base(db)
        {
        }

        public async Task<Specialty?> CheckExistingSpecialty(Specialty specialty)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.UserId == specialty.UserId && x.MedicalSpecialty == specialty.MedicalSpecialty);
        }
    }
}
