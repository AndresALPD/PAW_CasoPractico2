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
    }

}
