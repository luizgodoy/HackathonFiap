using Hackathon.Core.Models;
using Hackathon.Domain.Interfaces;

namespace Hackathon.Domain.Services
{
    public class UserServices : IUserServices
    {
        public Task Create(User user)
        {
            throw new NotImplementedException();
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
