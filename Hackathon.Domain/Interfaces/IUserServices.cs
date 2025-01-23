using Hackathon.Core.Models;

namespace Hackathon.Domain.Interfaces
{
    public interface IUserServices
    {
        Task Create(User user);
        Task<User> GetById(long id);
        Task<IEnumerable<User>> GetAll();
        Task Update(User user);
        Task Delete(long id);
    }
}