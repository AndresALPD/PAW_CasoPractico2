using Microsoft.IdentityModel.Tokens;
using PAWCP2.Core.Manager;
using PAWCP2.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PAWCP2.Api.Services
{
    public interface IAuthService
    {
        Task<Users> AuthenticateAsync(string username, string email);
        Task UpdateLastLoginAsync(int userId);
        Task<string> LoginWithTokenAsync(string username, string email);

    }

    public class AuthService : IAuthService
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IConfiguration _configuration;

        public AuthService(IUserBusiness userBusiness, IConfiguration configuration)
        {
            _userBusiness = userBusiness;
            _configuration = configuration;
        }

        // Método para generar token (sin guardar en BD)
        private string GenerateJwtToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token válido por 1 hora
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Users> AuthenticateAsync(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Username y email son requeridos.");

            // Usa el nuevo método de BusinessUser
            var user = await _userBusiness.GetByUsernameAndEmailAsync(username, email);

            if (user != null)
            {
                await UpdateLastLoginAsync(user.UserId); // Actualiza last login
            }

            return user; // Retorna el usuario (o null si falla)
        }

        public async Task<string> LoginWithTokenAsync(string username, string email)
        {
            var user = await AuthenticateAsync(username, email);
            return user != null ? GenerateJwtToken(user) : null;
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _userBusiness.GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLogin = DateTime.UtcNow; // Usar UtcNow para consistencia
                await _userBusiness.SaveAsync(user);
            }
        }
    }
}