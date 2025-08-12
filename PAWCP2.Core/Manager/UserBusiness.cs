using Microsoft.EntityFrameworkCore;
using PAWCP2.Models;
using PAWCP2.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAWCP2.Core.Manager
{
    public interface IUserBusiness
    {
        Task<IEnumerable<Users>> GetAllAsync();
        Task<Users> GetByIdAsync(int id);
        Task<bool> SaveAsync(Users user);
        Task<bool> DeleteAsync(Users user);
        Task<Users> GetByIdWithRolesAsync(int id);
        Task<Users> GetByUsernameAndEmailAsync(string username, string email);

    }

    public class BusinessUser : IUserBusiness
    {
        private readonly IRepositoryUser _repository;

        public BusinessUser(IRepositoryUser repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await _repository.ReadAsync();
        }

        public async Task<Users> GetByIdAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        public async Task<bool> SaveAsync(Users user)
        {
            bool isUpdating = user.UserId > 0;

            // Validar username único
            var existingUserWithSameUsername = await _repository.FindByUsernameAsync(user.Username);
            if (existingUserWithSameUsername != null && existingUserWithSameUsername.UserId != user.UserId)
            {
                throw new Exception("El username ya está en uso por otro usuario.");
            }

            // Validar email único
            var existingUserWithSameEmail = await _repository.FindByEmailAsync(user.Email);
            if (existingUserWithSameEmail != null && existingUserWithSameEmail.UserId != user.UserId)
            {
                throw new Exception("El email ya está en uso por otro usuario.");
            }

            if (isUpdating)
            {
                // Obtener usuario original para mantener CreatedAt
                var existingUser = await _repository.FindAsync(user.UserId);
                if (existingUser == null)
                    throw new Exception($"Usuario con Id {user.UserId} no existe para actualizar.");

                user.CreatedAt = existingUser.CreatedAt; // Mantener la fecha original
            }
            else
            {
                user.CreatedAt = DateTime.Now;
            }

            return await _repository.UpsertAsync(user, isUpdating);
        }

        public async Task<Users> GetByIdWithRolesAsync(int id)
        {
            return await _repository.FindWithRolesAsync(id);
        }


        public async Task<bool> DeleteAsync(Users user)
        {
            return await _repository.DeleteAsync(user);
        }
        public async Task<Users> GetByUsernameAndEmailAsync(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Username y email son requeridos.");

            // Busca primero por username
            var user = await _repository.FindByUsernameAsync(username);

            // Verifica si el email coincide y si está activo
            if (user != null && user.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && user.IsActive)
            {
                return user;
            }

            return null; 
        }
    }
}
