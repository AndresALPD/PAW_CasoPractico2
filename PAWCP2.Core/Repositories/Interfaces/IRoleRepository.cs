using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWCP2.Models;

namespace PAWCP2.Core.Repositories.Interfaces
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        Task<List<Role>> GetAllAsync();
        Task<bool> ExistsAsync(int roleId);
    }
}
