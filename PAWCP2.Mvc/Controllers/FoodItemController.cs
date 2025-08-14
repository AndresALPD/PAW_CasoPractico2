using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models;
using PAWCP2.Mvc.Models;
using PAWCP2.Mvc.Service;

namespace PAWCP2.Mvc.Controllers
{
    [Authorize]
    public class FoodItemController : Controller
    {
        private readonly IFoodItemService _foodItemService;

        public FoodItemController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        public async Task<IActionResult> Index(string filter)
        {
            IEnumerable<FoodItem> foodItems;

            switch (filter)// el R es Role 1,2,3
            {
                case "R1": 
                    foodItems = await _foodItemService.GetFoodItemsByRoleAsync(1);
                    break;
                case "R2":
                    foodItems = await _foodItemService.GetFoodItemsByRoleAsync(2);
                    break;
                case "R3": 
                    foodItems = await _foodItemService.GetFoodItemsByRoleAsync(3);
                    break;
                default:
                    foodItems = await _foodItemService.GetFoodItemsByRoleAsync(null);
                    break;
            }

            return View(foodItems);
        }
    }
}

