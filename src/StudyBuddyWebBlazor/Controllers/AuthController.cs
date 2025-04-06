using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Core.DTOs;
using StudyBuddy.Services.IServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddyWebBlazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository; 

        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository; 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            Console.WriteLine($"Login request: {dto.Identifier} / {dto.Password}");

            var result = await _authService.LoginAsync(dto);
            if (!result.IsSuccess)
            {
                Console.WriteLine("Login failed: " + string.Join(", ", result.Errors));
                return BadRequest(result);
            }

            var user = (await _userRepository.FindByNameAsync(dto.Identifier)).FirstOrDefault()
                       ?? await _userRepository.GetByEmailAsync(dto.Identifier);

            if (user == null)
                return BadRequest("User not found");

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email ?? "")
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(result);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("/auth/external-login")]
        public IActionResult ExternalLogin(string returnUrl = "/dashboard")
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = returnUrl
            };

            return Challenge(props, Microsoft.AspNetCore.Authentication.Google.GoogleDefaults.AuthenticationScheme);
        }
    }
}
