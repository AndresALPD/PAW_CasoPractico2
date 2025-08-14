using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PAWCP2.Models;
using PAWCP2.Data;

namespace PAWCP2.Repositories
{
    public interface IRepositoryUser
    {
        Task<bool> UpsertAsync(Users entity, bool isUpdating);
        Task<bool> CreateAsync(Users entity);
        Task<bool> DeleteAsync(Users entity);
        Task<IEnumerable<Users>> ReadAsync();
        Task<Users> FindAsync(int id);
        Task<bool> UpdateAsync(Users entity);
        Task<bool> UpdateManyAsync(IEnumerable<Users> entities);
        Task<bool> ExistsAsync(Users entity);
        Task<bool> CheckBeforeSavingAsync(Users entity);
        Task<IEnumerable<Users>> FilterAsync(Expression<Func<Users, bool>> predicate);
        Task<Users> FindWithRolesAsync(int id);
        Task<Users> FindByUsernameAsync(string username);
        Task<Users> FindByEmailAsync(string email);

    }

    public class RepositoryUser : RepositoryBase<Users>, IRepositoryUser
    {
        public RepositoryUser(PAWCP2DbContext ctx) : base(ctx)
        {
        }

        public async Task<bool> CheckBeforeSavingAsync(Users entity)
        {
            var exists = await ExistsAsync(entity);


            return await UpsertAsync(entity, exists);
        }

        public async Task<IEnumerable<Users>> FilterAsync(Expression<Func<Users, bool>> predicate)
        {
            return await DbContext.Users
                .Where(predicate)
                .Include(u => u.UserRoles) // Incluye relaciones si es necesario
                .ToListAsync();
        }
        public async Task<Users> FindWithRolesAsync(int id)
        {
            return await DbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }


        public async new Task<bool> ExistsAsync(Users entity)
        {
            return await DbContext.Users.AnyAsync(x => x.Username == entity.Username || x.Email == entity.Email);
        }
        public async Task UpdateAsync(Users user)
        {
            var trackedEntity = await DbContext.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (trackedEntity != null)
            {
                // Actualiza solo las propiedades necesarias
                DbContext.Entry(trackedEntity).CurrentValues.SetValues(user);
                await DbContext.SaveChangesAsync();
            }
        }
        public async Task<Users> FindByUsernameAsync(string username)
        {
            return await DbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Users> FindByEmailAsync(string email)
        {
            return await DbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
