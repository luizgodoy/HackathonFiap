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

    public async Task Create(string specialtyName, Guid doctorId)
    {
        var specialty = new Specialty()
        {
            MedicalSpecialty = specialtyName,
            UserId = doctorId,
        };
        var repositorySpecialty = await _specialtyRepository.CheckExistingSpecialty(specialty);

        if (repositorySpecialty != null)
            throw new Exception("Você já possui essa especialidade");

        await _specialtyRepository.Create(specialty);        
    }

}
