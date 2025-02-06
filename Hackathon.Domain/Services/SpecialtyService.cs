using Hackathon.Core.Models;
using Hackathon.Data.Interfaces;
using Hackathon.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Hackathon.Domain.Services;

public class SpecialtyService : ISpecialtyService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ISpecialtyRepository _specialtyRepository;

    public SpecialtyService(UserManager<User> userManager, SignInManager<User> signInManager, ISpecialtyRepository specialtyRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _specialtyRepository = specialtyRepository;
    }

    public Task Cancel(Specialty specialty, Guid patientId)
    {
        throw new NotImplementedException();
    }

    public async Task Create(string specialtyName, Guid doctorId)
    {
        var specialty = new Specialty()
        {
            MedicalSpecialty = specialtyName,
            UserId = doctorId,
        };

        await _specialtyRepository.Create(specialty);        
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Specialty>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Specialty>> GetAll(Guid? doctorId)
    {
        throw new NotImplementedException();
    }

    public Task Update(Specialty specialty)
    {
        throw new NotImplementedException();
    }
}
