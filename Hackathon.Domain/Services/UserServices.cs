using Hackathon.Core.DTO;
using Hackathon.Core.Models;
using Hackathon.Data.Interfaces;
using Hackathon.Domain.Interfaces;
using Hackathon.Domain.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hackathon.Domain.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;

        public UserServices(IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager, 
            RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task Create(User user)
        {
            var userValidator = new UserValidator();
            var result = userValidator.Validate(user);
            
            if (!result.IsValid)
                throw new Exception(result.Errors.FirstOrDefault()?.ErrorMessage);

            var createResult = await _userManager.CreateAsync(user, user.Password);

            if (!createResult.Succeeded)
            {
                var errorMessage = createResult.Errors.FirstOrDefault()?.Description;
                throw new Exception($"User creation failed: {errorMessage}");
            }

            if (!await _roleManager.RoleExistsAsync(user.Role.ToString()))
                await _roleManager.CreateAsync(new IdentityRole<Guid>(user.Role.ToString()));

            await _userManager.AddToRoleAsync(user, user.Role.ToString());
        }

        public async Task Delete(Guid id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                throw new Exception("Usuário não encontrado");

            await _userRepository.Delete(id);
        }

        public async Task<IEnumerable<User>> GetAll(Role? role = null)
        {
            return await _userRepository.GetAll(role);
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                throw new Exception("Usuário não encontrado");

            return user;
        }

        public async Task<string> LoginUser(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email) ?? throw new Exception("Email ou senha inválidos");

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded)
                throw new Exception("Email ou senha inválidos");

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return token;
        }

        public async Task Update(User user)
        {
            var existingUser = await _userRepository.GetById(user.Id);
            if (existingUser == null)
                throw new Exception("Usuário não encontrado");

            var userValidator = new UserValidator();
            var result = userValidator.Validate(user);

            if (!result.IsValid)
                throw new Exception(result.Errors.FirstOrDefault()?.ErrorMessage);

            await _userRepository.Update(user);
        }
        private string GenerateJwtToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("CPF", user.CPF)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var keyValue = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(keyValue))
            {
                throw new Exception("Chave JWT não foi configurada corretamente");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
