using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models;
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

            switch (filter)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStock(int FoodItemID, int QuantityInStock)
        {
            // Obtener el item desde la API
            var foodItems = await _foodItemService.GetFoodItemsByRoleAsync(null);
            var item = foodItems.FirstOrDefault(f => f.FoodItemID == FoodItemID);

            if (item == null)
            {
                TempData["Error"] = "El alimento no existe.";
                return RedirectToAction("Index");
            }

            // Actualizar solo QuantityInStock
            item.QuantityInStock = QuantityInStock;

            // Llamar al API para actualizar
            var result = await _foodItemService.UpdateQuantityInStockAsync(item);

            if (result)
                TempData["Success"] = "Stock actualizado correctamente";
            else
                TempData["Error"] = "Error al actualizar el stock";

            return RedirectToAction("Index");
        }


        //Cambiar estado restriccion cantidad es igual a 0
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

            // Actualizamos usando el nuevo método específico para IsActive
            var result = await _foodItemService.UpdateActiveStatusAsync(item);

            if (result)
                TempData["Success"] = item.IsActive ? "Alimento activado" : "Alimento desactivado";
            else
                TempData["Error"] = "Error al actualizar el estado";

            return RedirectToAction("Index");
        }



    }
}

