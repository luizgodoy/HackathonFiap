using Hackathon.Core.Models;

namespace Hackathon.Data.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(long id);
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(Guid id);
        Task<int> SaveChanges();        
    }
}