using Hackathon.Core.Models;
using Hackathon.Data.Interfaces;
using Hackathon.Domain.Interfaces;
using Hackathon.Domain.Validators;

namespace Hackathon.Domain.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Create(User user)
        {
            // Regras adicionais antes da validação
            if (user.Role == Role.Patient && !string.IsNullOrEmpty(user.CRM))
                throw new Exception("Pacientes não devem possuir CRM");

            if (user.Role == Role.Doctor && string.IsNullOrEmpty(user.CRM))
                throw new Exception("O CRM é obrigatório para médicos");

            var userValidator = new UserValidator();
            var result = userValidator.Validate(user);

            if (!result.IsValid)
                throw new Exception(result.Errors.FirstOrDefault()?.ErrorMessage);

            await _userRepository.Create(user);
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

        public async Task Update(User user)
        {
            var existingUser = await _userRepository.GetById(user.Id);
            if (existingUser == null)
                throw new Exception("Usuário não encontrado");

            user.Password = existingUser.Password;

            var userValidator = new UserValidator();
            var result = userValidator.Validate(user);

            if (!result.IsValid)
                throw new Exception(result.Errors.FirstOrDefault()?.ErrorMessage);

            await _userRepository.Update(user);
        }
    }
}
