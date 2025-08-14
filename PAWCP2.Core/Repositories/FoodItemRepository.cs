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
        Task<IEnumerable<string>> GetDistinctCategoriesAsync();
        Task<IEnumerable<string>> GetDistinctBrandsAsync();
        Task<IEnumerable<string>> GetDistinctSuppliersAsync();
        Task<bool> ToggleActiveStatusAsync(int foodItemId);
        Task<bool> UpdateQuantityAsync(int foodItemId, int quantity);
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

        public async Task<bool> ToggleActiveStatusAsync(int foodItemId)
        {
            var item = await _context.FoodItems.FindAsync(foodItemId);
            if (item == null) return false;

            // Solo permite desactivar si cantidad es 0
            if (item.IsActive == false || item.QuantityInStock == 0)
            {
                item.IsActive = !item.IsActive;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateQuantityAsync(int foodItemId, int quantity)
        {
            var item = await _context.FoodItems.FindAsync(foodItemId);
            if (item == null) return false;

            item.QuantityInStock = quantity;

            // Si se establece cantidad a 0, desactivar automáticamente
            if (quantity == 0)
            {
                item.IsActive = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
