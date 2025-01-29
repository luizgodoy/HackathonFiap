using Hackathon.Core.Models;

namespace Hackathon.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAll(Role? role = null);
    }
}