using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Core.Manager;
using PAWCP2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAWCP2.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness _businessUser;

        public UsersController(IUserBusiness businessUser)
        {
            _businessUser = businessUser;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAll()
        {
            var users = await _businessUser.GetAllAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetById(int id)
        {
            var user = await _businessUser.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Users user)
        {
            var result = await _businessUser.SaveAsync(user);
            if (!result)
                return BadRequest("No se pudo crear el usuario.");

            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }

        /*
          {
          "username": "mariagarcia",
          "email": "mariagarcia@example.com",
          "fullName": "Maria Garcia",
          "isActive": true,
          "createdAt": "2025-08-07T11:30:00Z",
          "lastLogin": "2025-08-07T11:35:00Z",
          "userRoles": [
            {
              "roleId": 1,
              "description": "Admin"
            },
            {
              "roleId": 3,
              "description": "Editor"
            }
          ]
        }
        
         */


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Users user)
        {
            if (id != user.UserId)
                return BadRequest("ID de usuario inválido.");

            // Obtener usuario existente con roles
            var existingUser = await _businessUser.GetByIdWithRolesAsync(id);
            if (existingUser == null)
                return NotFound("Usuario no existe.");

            // Actualizar campos simples
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.FullName = user.FullName;
            existingUser.IsActive = user.IsActive;
            existingUser.LastLogin = user.LastLogin; // si usas este campo

            // Actualizar roles manualmente
            existingUser.UserRoles.Clear();
            foreach (var role in user.UserRoles)
            {
                existingUser.UserRoles.Add(role);
            }

            // Guardar cambios usando SaveAsync sin modificarlo
            var result = await _businessUser.SaveAsync(existingUser);
            if (!result)
                return BadRequest("No se pudo actualizar el usuario.");

            return NoContent();
        }











    }



}
