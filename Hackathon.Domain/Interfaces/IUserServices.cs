using Hackathon.Core.DTO;
using Hackathon.Core.Models;

namespace Hackathon.Domain.Interfaces
{
    public interface IUserServices
    {
        Task Create(User user);

        Task Update(User user);

        Task<IEnumerable<User>> GetAll(Role? role = null);

        Task<User> GetById(Guid id);

        Task Delete(Guid id);
    }
}