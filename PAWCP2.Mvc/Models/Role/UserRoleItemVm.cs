using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PAWCP2.Mvc.Models.Role
{
    public class UserRoleItemVm
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public int? RoleId { get; set; }
        public List<SelectListItem> Roles { get; set; } = new();
        public int? CurrentRoleId { get; set; }

    }
}
