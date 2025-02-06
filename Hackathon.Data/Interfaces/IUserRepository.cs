using Hackathon.Core.Models;

namespace Hackathon.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll(Role? role, string specialty);        
        Task<User> GetById(Guid id);
        Task Update(User entity);
        Task Delete(Guid id);        
    }
}