using Microsoft.AspNetCore.Mvc;
using PAWCP2.Core.Manager;
using PAWCP2.Models;

namespace PAWCP2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly IBusinessFoodItem _business;

        public FoodItemController(IBusinessFoodItem business)
        {
            _business = business;
        }

        // GET: api/fooditem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetAll()
        {
            var foodItems = await _business.GetAllAsync();
            return Ok(foodItems);
        }

        // GET: api/fooditem/byrole/2
        [HttpGet("byrole/{roleId}")]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetByRole(int roleId)
        {
            var foodItems = await _business.GetByRoleAsync(roleId);
            return Ok(foodItems);
        }

        [HttpPost("advancedsearch")]
        public async Task<ActionResult<IEnumerable<FoodItem>>> AdvancedSearch([FromBody] FoodItemSearchCriteria criteria)
        {
            var foodItems = await _business.AdvancedSearchAsync(criteria);
            return Ok(foodItems);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            return Ok(await _business.GetCategoriesAsync());
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<string>>> GetBrands()
        {
            return Ok(await _business.GetBrandsAsync());
        }

        [HttpGet("suppliers")]
        public async Task<ActionResult<IEnumerable<string>>> GetSuppliers()
        {
            return Ok(await _business.GetSuppliersAsync());
        }

        [HttpPost("toggleactive/{id}")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var result = await _business.ToggleActiveStatusAsync(id);
            return result ? Ok() : BadRequest("No se puede desactivar un ítem con cantidad mayor a 0");
        }

        [HttpPost("updatequantity/{id}")]
        public async Task<IActionResult> UpdateQuantity(int id, [FromBody] int quantity)
        {
            var result = await _business.UpdateQuantityAsync(id, quantity);
            return result ? Ok() : BadRequest("Error al actualizar la cantidad");
        }


    }

}
