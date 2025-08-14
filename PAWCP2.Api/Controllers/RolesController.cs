using Microsoft.AspNetCore.Mvc;
using PAWCP2.Core.Repositories.Interfaces;
using PAWCP2.Models;

namespace PAWCP2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roles;

        public RolesController(IRoleRepository roles)
        {
            _roles = roles;
        }

        // GET: /api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAll()
        {
            var list = await _roles.GetAllAsync();
            return Ok(list);
        }

        // GET: /api/Roles/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Role>> GetById(int id)
        {
            var item = (await _roles.GetAllAsync()).FirstOrDefault(r => r.RoleId == id);
            return item is null ? NotFound() : Ok(item);
        }

        // (Opcional) POST/PUT/DELETE si vas a administrar roles desde el API
    }
}
