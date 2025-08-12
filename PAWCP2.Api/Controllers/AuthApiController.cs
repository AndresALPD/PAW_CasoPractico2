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

        // POST: api/AuthApi/login (Versión original sin token)
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

        // POST: api/AuthApi/token (Nuevo endpoint para obtener JWT)
        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Success = false, Message = "Datos inválidos." });
            }

            var token = await _authService.LoginWithTokenAsync(request.Username, request.Email);

            if (token == null)
            {
                return Unauthorized(new { Success = false, Message = "Credenciales incorrectas." });
            }

            return Ok(new
            {
                Success = true,
                Token = token
            });
        }

        // POST: api/AuthApi/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
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