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
            var userValidator = new UserValidator();
            var result = userValidator.Validate(user);

            if (!result.IsValid)
                throw new Exception(result.Errors.FirstOrDefault()?.ErrorMessage);

            await _userRepository.Create(user);
        }

        public async Task Delete(Guid id)
        {
            await _userRepository.Delete(id);
        }

        public async Task<IEnumerable<User>> GetAll(Role? role = null)
        {
           return await _userRepository.GetAll(role);
        }

        public async Task<User> GetById(Guid id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task Update(User user)
        {
            var userValidator = new UserValidator();
            var result = userValidator.Validate(user);

            if (!result.IsValid)
                throw new Exception(result.Errors.FirstOrDefault()?.ErrorMessage);

            await _userRepository.Update(user);
        }
    }
}