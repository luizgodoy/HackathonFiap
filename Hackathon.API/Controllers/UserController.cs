using Hackathon.Core.Models;
using Hackathon.Domain.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Hackathon.Core.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Hackathon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly IMapper _mapper;        

        public UserController(IUserServices userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

       
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] NewUserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto); 
                await _userService.Create(user);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto login)
        {
            try
            {
                return await _userService.LoginUser(login);

            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] Role? role = null)
        {
            try
            {
                var users = await _userService.GetAll(role);
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users); 
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetById(id);
                if (user == null) return NotFound(new { message = "Usuário não encontrado" });

                var userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto userDto)
        {
            try
            {
                if (id != userDto.Id)
                    return BadRequest(new { message = "ID informado não corresponde ao usuário." });

              
                var existingUser = await _userService.GetById(id);
                if (existingUser == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                var user = _mapper.Map<User>(userDto);

                await _userService.Update(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var existingUser = await _userService.GetById(id);
                if (existingUser == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                await _userService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
