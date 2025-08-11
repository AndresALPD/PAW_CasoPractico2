using Microsoft.AspNetCore.Mvc;
using PAWCP2.Api.Services;
using PAWCP2.Models;
using System.Threading.Tasks;

namespace PAWCP2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/AuthApi/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Success = false, Message = "Datos inválidos." });
            }

            var user = await _authService.AuthenticateAsync(request.Username, request.Email);
            
            if (user == null)
            {
                return Unauthorized(new { Success = false, Message = "Credenciales incorrectas o usuario inactivo." });
            }

            await _authService.UpdateLastLoginAsync(user.UserId);

            // Respuesta exitosa (puedes incluir un token JWT en el futuro)
            return Ok(new 
            {
                Success = true,
                User = new 
                {
                    user.UserId,
                    user.Username,
                    user.Email,
                    user.FullName
                }
            });
        }

        // POST: api/AuthApi/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // En una API REST pura, el logout suele manejarse en el cliente (eliminando el token).
            return Ok(new { Success = true, Message = "Sesión cerrada (client-side)." });
        }
    }

    // Modelo para el request de login
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}