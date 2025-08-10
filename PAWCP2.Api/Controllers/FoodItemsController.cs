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
    }

}
