using System.Net.Http.Json;

namespace PAWCP2.Mvc.Services
{
    public record RoleDto(int RoleId, string RoleName);

    public interface IRolesApiClient
    {
        Task<List<RoleDto>> GetAllAsync();
    }

    public class RolesApiClient : IRolesApiClient
    {
        private readonly HttpClient _http;
        public RolesApiClient(HttpClient http) { _http = http; }

        public async Task<List<RoleDto>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<RoleDto>>("api/Roles") ?? new();
    }
}
