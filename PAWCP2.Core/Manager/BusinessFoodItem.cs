using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWCP2.Core.Repositories;
using PAWCP2.Models;

namespace PAWCP2.Core.Manager
{
    public interface IBusinessFoodItem
    {
        Task<IEnumerable<FoodItem>> GetAllAsync();
        Task<IEnumerable<FoodItem>> GetByRoleAsync(int roleId);
        Task<IEnumerable<FoodItem>> AdvancedSearchAsync(FoodItemSearchCriteria criteria);
        Task<IEnumerable<string>> GetCategoriesAsync();
        Task<IEnumerable<string>> GetBrandsAsync();
        Task<IEnumerable<string>> GetSuppliersAsync();
        Task<bool> ToggleActiveStatusAsync(int foodItemId);
        Task<bool> UpdateQuantityAsync(int foodItemId, int quantity);
    }

    public class BusinessFoodItem : IBusinessFoodItem
    {
        private readonly IFoodItemRepository _repository;

        public BusinessFoodItem(IFoodItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FoodItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<FoodItem>> GetByRoleAsync(int roleId)
        {
            var all = await _repository.GetByRoleIdAsync(roleId);

            return roleId switch
            {
                1 => all,
                2 => all.Where(f => f.Category != null),
                3 => all.Where(f => f.IsActive == true),
                _ => Enumerable.Empty<FoodItem>()
            };
        }

        public async Task<IEnumerable<FoodItem>> AdvancedSearchAsync(FoodItemSearchCriteria criteria)
        {
            var query = (await _repository.GetAllAsync()).AsQueryable();

            if (criteria.RoleId.HasValue)
                query = (await _repository.GetByRoleIdAsync(criteria.RoleId.Value)).AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Category))
                query = query.Where(f => f.Category == criteria.Category);

            if (!string.IsNullOrEmpty(criteria.Brand))
                query = query.Where(f => f.Brand == criteria.Brand);

            if (criteria.MinPrice.HasValue)
                query = query.Where(f => f.Price >= criteria.MinPrice);

            if (criteria.MaxPrice.HasValue)
                query = query.Where(f => f.Price <= criteria.MaxPrice);

            if (criteria.ExpirationDateBefore.HasValue)
                query = query.Where(f => f.ExpirationDate <= criteria.ExpirationDateBefore);

            if (criteria.IsPerishable.HasValue)
                query = query.Where(f => f.IsPerishable == criteria.IsPerishable);

            if (criteria.MaxCalories.HasValue)
                query = query.Where(f => f.CaloriesPerServing <= criteria.MaxCalories);

            if (!string.IsNullOrEmpty(criteria.Supplier))
                query = query.Where(f => f.Supplier == criteria.Supplier);

            if (criteria.IsActive.HasValue)
                query = query.Where(f => f.IsActive == criteria.IsActive);

            return query.ToList();
        }

        public async Task<bool> ToggleActiveStatusAsync(int foodItemId)
        {
            return await _repository.ToggleActiveStatusAsync(foodItemId);
        }

        public async Task<bool> UpdateQuantityAsync(int foodItemId, int quantity)
        {
            return await _repository.UpdateQuantityAsync(foodItemId, quantity);
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync() => await _repository.GetDistinctCategoriesAsync();
        public async Task<IEnumerable<string>> GetBrandsAsync() => await _repository.GetDistinctBrandsAsync();
        public async Task<IEnumerable<string>> GetSuppliersAsync() => await _repository.GetDistinctSuppliersAsync();
    }

}
