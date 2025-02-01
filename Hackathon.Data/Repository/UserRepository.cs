using Hackathon.Core.Models;
using Hackathon.Data.Context;
using Hackathon.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HackathonDbContext _context;
        private readonly DbSet<User> DbSet;

        public UserRepository(HackathonDbContext context)
        {
            _context = context;
            DbSet = _context.Set<User>();
        }

        public async Task Delete(Guid id)
        {
            var user = DbSet.Find(id);
            if (user is null)
                throw new Exception("Usuário não encontrado");

            DbSet.Remove(user);
            await SaveChanges();
        }

        public async Task<IEnumerable<User>> GetAll(Role? role = null)
        {
            
            var query = _context.Users.AsQueryable();

            if (role.HasValue)
            {
                query = query.Where(user => user.Role == role.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task Update(User entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        private async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}