using StudyBuddy.Core.DTOs;
using System.Net.Http;
using System.Net.Http.Json;

namespace StudyBuddyWebBlazor.Services
{
    public class ApiUserService
    {
        private readonly HttpClient _httpClient;

        public ApiUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto login)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", login);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<AuthResultDto>();
                return error!;
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResultDto>();
            return result!;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
            return await response.Content.ReadFromJsonAsync<AuthResultDto>() ?? new AuthResultDto();
        }
    }
}
