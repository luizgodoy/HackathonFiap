using Hackathon.Core.Models;
using Hackathon.Data.Context;
using Hackathon.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly HackathonDbContext _context;

        public UserRepository(HackathonDbContext context) : base(context)
        {
            _context = context;
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
    }
}