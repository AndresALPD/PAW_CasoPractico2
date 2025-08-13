using Microsoft.AspNetCore.Mvc;
using PAWCP2.Manager;
using PAWCP2.Mvc.Models.Role;

namespace PAWCP2.Mvc.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly IUserRoleManager _manager;

        public UserRolesController(IUserRoleManager manager) => _manager = manager;

        public async Task<IActionResult> Index()
        {
            var usersRes = await _manager.GetUsersAsync();
            if (!usersRes.Success) { TempData["Error"] = usersRes.Error; return View(new List<UserRoleItemVm>()); }

            var rolesRes = await _manager.GetRolesAsSelectAsync();
            if (!rolesRes.Success) { TempData["Error"] = rolesRes.Error; return View(new List<UserRoleItemVm>()); }

            var list = new List<UserRoleItemVm>();
            foreach (var u in usersRes.Data!)
            {
                var cur = await _manager.GetCurrentRoleIdAsync(u.UserId);
                var vm = new UserRoleItemVm
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    RoleId = cur.Success ? cur.Data : null,
                    Roles = rolesRes.Data!
                };
                list.Add(vm);
            }
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int userId, int roleId)
        {
            var res = await _manager.AssignRoleAsync(userId, roleId);
            if (!res.Success) TempData["Error"] = res.Error;
            else TempData["Ok"] = "Rol asignado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
