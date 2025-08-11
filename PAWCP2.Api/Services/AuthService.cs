using PAWCP2.Core.Manager;
using PAWCP2.Models;
using System;
using System.Threading.Tasks;

namespace PAWCP2.Api.Services
{
    public interface IAuthService
    {
        Task<Users> AuthenticateAsync(string username, string email);
        Task UpdateLastLoginAsync(int userId);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserBusiness _userBusiness;

        public AuthService(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
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