using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Core.Manager;
using PAWCP2.Models;
using PAWCP2.Mvc.Service;

namespace PAWCP2.Mvc.Controllers
{
    [Authorize]
    public class FoodItemController : Controller
    {
        private readonly IFoodItemService _foodItemService;
        private readonly IBusinessFoodItem _businessFoodItem;

        public FoodItemController(IFoodItemService foodItemService, IBusinessFoodItem businessFoodItem)
        {
            _foodItemService = foodItemService;
            _businessFoodItem = businessFoodItem;
        }

        public async Task<IActionResult> Index(string filter, FoodItemSearchCriteria searchCriteria, bool clear = false)
        {
            if (clear)
            {
                return RedirectToAction("Index");
            }

            // Manejo de fechas si viene del formulario
            if (Request.HasFormContentType && !string.IsNullOrEmpty(Request.Form["searchCriteria.ExpirationDateBefore"]))
            {
                if (DateOnly.TryParse(Request.Form["searchCriteria.ExpirationDateBefore"], out var date))
                {
                    searchCriteria.ExpirationDateBefore = date;
                }
            }

            ViewBag.Categories = await _businessFoodItem.GetCategoriesAsync();
            ViewBag.Brands = await _businessFoodItem.GetBrandsAsync();
            ViewBag.Suppliers = await _businessFoodItem.GetSuppliersAsync();

            IEnumerable<FoodItem> foodItems;

            if (searchCriteria != null && !IsEmptySearch(searchCriteria))
            {
                searchCriteria.RoleId = filter switch
                {
                    "R1" => 1,
                    "R2" => 2,
                    "R3" => 3,
                    _ => null
                };

                foodItems = await _businessFoodItem.AdvancedSearchAsync(searchCriteria);
            }
            else
            {
                switch (filter)
                {
                    case "R1": foodItems = await _foodItemService.GetFoodItemsByRoleAsync(1); break;
                    case "R2": foodItems = await _foodItemService.GetFoodItemsByRoleAsync(2); break;
                    case "R3": foodItems = await _foodItemService.GetFoodItemsByRoleAsync(3); break;
                    default: foodItems = await _foodItemService.GetFoodItemsByRoleAsync(null); break;
                }
            }

            var viewModel = new FoodItemListViewModel
            {
                FoodItems = foodItems,
                SearchCriteria = searchCriteria ?? new FoodItemSearchCriteria()
            };

            return View(viewModel);
        }

        private bool IsEmptySearch(FoodItemSearchCriteria criteria)
        {
            return string.IsNullOrEmpty(criteria.Category) &&
                   string.IsNullOrEmpty(criteria.Brand) &&
                   !criteria.MinPrice.HasValue &&
                   !criteria.MaxPrice.HasValue &&
                   !criteria.ExpirationDateBefore.HasValue &&
                   !criteria.IsPerishable.HasValue &&
                   !criteria.MaxCalories.HasValue &&
                   string.IsNullOrEmpty(criteria.Supplier) &&
                   !criteria.IsActive.HasValue;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStock(int FoodItemID, int QuantityInStock)
        {
            var foodItems = await _foodItemService.GetFoodItemsByRoleAsync(null);
            var item = foodItems.FirstOrDefault(f => f.FoodItemID == FoodItemID);

            if (item == null)
            {
                TempData["Error"] = "El alimento no existe.";
                return RedirectToAction("Index");
            }

            item.QuantityInStock = QuantityInStock;
            var result = await _foodItemService.UpdateQuantityInStockAsync(item);

            if (result)
                TempData["Success"] = "Stock actualizado correctamente";
            else
                TempData["Error"] = "Error al actualizar el stock";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeActiveStatus(int FoodItemID)
        {
            var foodItems = await _foodItemService.GetFoodItemsByRoleAsync(null);
            var item = foodItems.FirstOrDefault(f => f.FoodItemID == FoodItemID);

            if (item == null)
            {
                TempData["Error"] = "El alimento no existe.";
                return RedirectToAction("Index");
            }

            if (item.IsActive && item.QuantityInStock > 0)
            {
                TempData["Error"] = "No se puede desactivar un alimento que tiene stock disponible.";
                return RedirectToAction("Index");
            }

            item.IsActive = !item.IsActive;
            var result = await _foodItemService.UpdateActiveStatusAsync(item);

            if (result)
                TempData["Success"] = item.IsActive ? "Alimento activado" : "Alimento desactivado";
            else
                TempData["Error"] = "Error al actualizar el estado";

            return RedirectToAction("Index");
        }
    }
}
