using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWCP2.Core.Architecture;
using PAWCP2.Core.Repositories.Interfaces;
using PAWCP2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PAWCP2.Core.Manager
{
    public interface IUserRoleManager
    {
        Task<OperationResult<List<Users>>> GetUsersAsync();
        Task<OperationResult<List<SelectListItem>>> GetRolesAsSelectAsync();
        Task<OperationResult<int>> GetCurrentRoleIdAsync(int userId);
        Task<OperationResult<bool>> AssignRoleAsync(int userId, int roleId);
    }

    public class UserRoleManager : IUserRoleManager
    {
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly IUserRoleRepository _userRoles;

        public UserRoleManager(
            IUserRepository users,
            IRoleRepository roles,
            IUserRoleRepository userRoles)
        {
            _users = users;
            _roles = roles;
            _userRoles = userRoles;
        }

        public async Task<OperationResult<List<Users>>> GetUsersAsync()
        {
            try
            {
                var list = await _users.GetAllAsync();
                return OperationResult<List<Users>>.Ok(list);
            }
            catch (Exception ex)
            {
                return OperationResult<List<Users>>.Fail($"Error obteniendo usuarios: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<SelectListItem>>> GetRolesAsSelectAsync()
        {
            try
            {
                var roles = await _roles.GetAllAsync();
                var items = roles
                    .Select(r => new SelectListItem { Value = r.RoleId.ToString(), Text = r.RoleName })
                    .ToList();
                return OperationResult<List<SelectListItem>>.Ok(items);
            }
            catch (Exception ex)
            {
                return OperationResult<List<SelectListItem>>.Fail($"Error obteniendo roles: {ex.Message}");
            }
        }

        public async Task<OperationResult<int>> GetCurrentRoleIdAsync(int userId)
        {
            try
            {
                var current = await _userRoles.GetCurrentAsync(userId);
                return OperationResult<int>.Ok(current?.RoleId ?? 0);
            }
            catch (Exception ex)
            {
                return OperationResult<int>.Fail($"Error obteniendo rol actual: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> AssignRoleAsync(int userId, int roleId)
        {
            try
            {
                var userExists = await _users.ExistsAsync(userId);
                if (!userExists) return OperationResult<bool>.Fail("Usuario no existe.");

                var roleExists = await _roles.ExistsAsync(roleId);
                if (!roleExists) return OperationResult<bool>.Fail("Rol no existe.");

                var ok = await _userRoles.ReplaceAsync(userId, roleId);
                return ok
                    ? OperationResult<bool>.Ok(true)
                    : OperationResult<bool>.Fail("No se pudo guardar la asignación.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error al asignar rol: {ex.Message}");
            }
        }
    }
}
