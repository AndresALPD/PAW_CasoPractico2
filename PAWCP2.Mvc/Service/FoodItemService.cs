using System.Text.Json;
using PAWCP2.Models;

namespace PAWCP2.Mvc.Service
{
    public interface IFoodItemService
    {
        Task<IEnumerable<FoodItem>> GetFoodItemsByRoleAsync(int? roleId);
        Task<IEnumerable<FoodItem>> AdvancedSearchAsync(FoodItemSearchCriteria criteria);
    }

    public class FoodItemService : IFoodItemService
    {
        private readonly HttpClient _httpClient;

        public FoodItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7289/"); //cambiar la ruta si no les sirve
        }

        public async Task<IEnumerable<FoodItem>> GetFoodItemsByRoleAsync(int? roleId)
        {
            string url = "api/fooditem";

            if (roleId.HasValue)
                url += $"/byrole/{roleId.Value}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<FoodItem>();

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var foodItems = JsonSerializer.Deserialize<IEnumerable<FoodItem>>(content, options);

            return foodItems ?? Enumerable.Empty<FoodItem>();
        }

        public async Task<IEnumerable<FoodItem>> AdvancedSearchAsync(FoodItemSearchCriteria criteria)
        {
            var response = await _httpClient.PostAsJsonAsync("api/fooditem/advancedsearch", criteria);

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<FoodItem>();

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<IEnumerable<FoodItem>>(content, options)
                   ?? Enumerable.Empty<FoodItem>();
        }
    }

}
