using Hackathon.Core.Models;
using Hackathon.Data.Context;
using Hackathon.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        protected UserRepository(HackathonDbContext db) : base(db)
        {
        }
    }
}
