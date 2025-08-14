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
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(PAWCP2DbContext context) : base(context) { }

        public async Task<List<Role>> GetAllAsync() =>
            await DbContext.Roles.AsNoTracking().OrderBy(r => r.RoleName).ToListAsync();

        public async Task<bool> ExistsAsync(int roleId) =>
            await DbContext.Roles.AnyAsync(r => r.RoleId == roleId);
    }
}
