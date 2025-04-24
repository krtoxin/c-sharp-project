using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using StudyBuddy.Core.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomAuthenticationStateProvider(ProtectedLocalStorage localStorage, IHttpContextAccessor httpContextAccessor)
    {
        _localStorage = localStorage;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sessionResult = await _localStorage.GetAsync<AuthResultDto>("sessionState");
        var sessionModel = sessionResult.Success ? sessionResult.Value : null;

        if (sessionModel != null && !string.IsNullOrEmpty(sessionModel.Token))
        {
            var identity = GetClaimsIdentity(sessionModel.Token);
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        var httpUser = _httpContextAccessor.HttpContext?.User;
        if (httpUser != null && httpUser.Identity?.IsAuthenticated == true)
        {
            return new AuthenticationState(httpUser);
        }
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task MarkUserAsAuthenticated(AuthResultDto model)
    {
        await _localStorage.SetAsync("sessionState", model);
        var identity = GetClaimsIdentity(model.Token);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.DeleteAsync("sessionState");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }

    private ClaimsIdentity GetClaimsIdentity(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var claims = jwtToken.Claims;
        return new ClaimsIdentity(claims, "jwt");
    }
}
