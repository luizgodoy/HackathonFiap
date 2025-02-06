using AutoMapper;
using Hackathon.Core.Models;
using Hackathon.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hackathon.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpecialtyController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ISpecialtyService _specialtyService;
    private readonly IMapper _mapper;

    public SpecialtyController(ISpecialtyService specialtyService, IMapper mapper, UserManager<User> userManager)
    {
        _mapper = mapper;
        _specialtyService = specialtyService;
        _userManager = userManager;
    }


    [HttpPost("specialty")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> AddSpecialty([FromBody] string specialtyName)
    {
        try
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByNameAsync(userEmail) ?? throw new Exception("Usuário não encontrado");

            await _specialtyService.Create(specialtyName, user.Id);
            return Ok("Ok");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
