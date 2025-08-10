using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Data;
using PAWCP2.Models;

namespace PAWCP2.Core.Repositories
{
    public interface IFoodItemRepository
    {
        Task<IEnumerable<FoodItem>> GetAllAsync();
        Task<IEnumerable<FoodItem>> GetByRoleIdAsync(int roleId);
    }

    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly PAWCP2DbContext _context;

        public FoodItemRepository(PAWCP2DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FoodItem>> GetAllAsync()
        {
            return await _context.FoodItems
                .ToListAsync();
        }

        public async Task<IEnumerable<FoodItem>> GetByRoleIdAsync(int roleId)
        {
            return await _context.FoodItems
                .Where(f => f.RoleId == roleId)
                .ToListAsync();
        }
    }

}
