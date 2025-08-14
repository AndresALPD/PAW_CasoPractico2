using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Repositories.Interfaces;
using PAWCP2.Data;
using PAWCP2.Models;

namespace PAWCP2.Core.Repositories.Implementations
{
    public class UserRepository : RepositoryBase<Users>, IUserRepository
    {
        public UserRepository(PAWCP2DbContext context) : base(context) { }

        public async Task<List<Users>> GetAllAsync() =>
            await DbContext.Users.AsNoTracking().OrderBy(u => u.Username).ToListAsync();

        public async Task<bool> ExistsAsync(int userId) =>
            await DbContext.Users.AnyAsync(u => u.UserId == userId);
    }
}
