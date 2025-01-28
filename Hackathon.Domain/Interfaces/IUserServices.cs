using Hackathon.Core.Models;

namespace Hackathon.Domain.Interfaces
{
    public interface IUserServices
    {
        Task Create(User user);
        Task<User> GetById(Guid id);
        Task<IEnumerable<User>> GetAll();
        Task Update(User user);
        Task Delete(Guid id);
    }
}