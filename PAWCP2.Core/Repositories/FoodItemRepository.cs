using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<FoodItem?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(FoodItem item);
        Task<IEnumerable<string>> GetDistinctCategoriesAsync();
        Task<IEnumerable<string>> GetDistinctBrandsAsync();
        Task<IEnumerable<string>> GetDistinctSuppliersAsync();
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

        public async Task<FoodItem?> GetByIdAsync(int id)
        {
            return await _context.FoodItems.FirstOrDefaultAsync(f => f.FoodItemID == id);
        }

        public async Task<bool> UpdateAsync(FoodItem item)
        {
            _context.FoodItems.Update(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<string>> GetDistinctCategoriesAsync()
        {
            return await _context.FoodItems
                .Where(f => f.Category != null)
                .Select(f => f.Category)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctBrandsAsync()
        {
            return await _context.FoodItems
                .Where(f => f.Brand != null)
                .Select(f => f.Brand)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctSuppliersAsync()
        {
            return await _context.FoodItems
                .Where(f => f.Supplier != null)
                .Select(f => f.Supplier)
                .Distinct()
                .ToListAsync();
        }
    }
}