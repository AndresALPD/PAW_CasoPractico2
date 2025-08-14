using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // PUT: api/fooditem/{id}/stock
        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] FoodItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (item == null || id != item.FoodItemID)
                return BadRequest("Datos inválidos o el ID no coincide.");

            var existingItem = await _business.GetByIdAsync(id);
            if (existingItem == null)
                return NotFound("El alimento no existe.");

            existingItem.QuantityInStock = item.QuantityInStock;

            var result = await _business.UpdateAsync(existingItem);

            if (!result)
                return StatusCode(500, "Error al actualizar el stock en la base de datos.");

            return NoContent();
        }

        // PUT: api/fooditem/{id}/active
        [HttpPut("{id}/active")]
        public async Task<IActionResult> UpdateActive(int id, [FromBody] FoodItem item)
        {
            if (item == null || id != item.FoodItemID)
                return BadRequest("Datos inválidos o ID no coincide.");

            var existingItem = await _business.GetByIdAsync(id);
            if (existingItem == null)
                return NotFound("El alimento no existe.");

            existingItem.IsActive = item.IsActive;

            var result = await _business.UpdateAsync(existingItem);

            if (!result)
                return StatusCode(500, "Error al actualizar el estado en la base de datos.");

            return NoContent();
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
    }
}