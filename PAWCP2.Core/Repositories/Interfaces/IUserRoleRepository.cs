using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWCP2.Models;

namespace PAWCP2.Core.Repositories.Interfaces
{
    public interface IUserRoleRepository : IRepositoryBase<UserRole>
    {
        Task<UserRole?> GetCurrentAsync(int userId);
        Task<bool> ReplaceAsync(int userId, int roleId); // elimina previas y pone la nueva
    }
}
