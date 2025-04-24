using Microsoft.AspNetCore.Components.Authorization;
using StudyBuddy.Core.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudyBuddyWebBlazor.Services
{
    public class ApiUserService
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthenticationStateProvider _authProvider;

        public ApiUserService(HttpClient httpClient, AuthenticationStateProvider authProvider)
        {
            _httpClient = httpClient;
            _authProvider = (CustomAuthenticationStateProvider)authProvider;
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new() { $"HTTP Error {response.StatusCode}" }
                };
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResultDto>();

            if (result is null)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new() { "Server returned empty response" }
                };
            }

            if (result.IsSuccess)
            {
                await _authProvider.MarkUserAsAuthenticated(result);
            }

            return result;
        }


        public async Task<AuthResultDto> RegisterAsync(RegisterDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
            return await response.Content.ReadFromJsonAsync<AuthResultDto>() ?? new AuthResultDto();
        }

        public async Task LogoutAsync()
        {
            await _authProvider.MarkUserAsLoggedOut();
        }

        public async Task<AuthResultDto> SendPasswordResetCodeAsync(string email)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/send-reset-code", new { Email = email });
            return await response.Content.ReadFromJsonAsync<AuthResultDto>() ?? new AuthResultDto();
        }

        public async Task<AuthResultDto> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
            return await response.Content.ReadFromJsonAsync<AuthResultDto>() ?? new AuthResultDto();
        }

    }
}
