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
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(PAWCP2DbContext context) : base(context) { }

        public async Task<UserRole?> GetCurrentAsync(int userId) =>
            await DbContext.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);

        // Reemplaza cualquier asignación previa por la nueva (PK compuesta UserId+RoleId)
        public async Task<bool> ReplaceAsync(int userId, int roleId)
        {
            var prev = await DbContext.UserRoles.Where(x => x.UserId == userId).ToListAsync();
            if (prev.Count > 0) DbContext.UserRoles.RemoveRange(prev);

            await DbContext.UserRoles.AddAsync(new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                Description = null
            });

            return await SaveAsync();
        }
    }
}
