using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Core.DTOs;
using StudyBuddy.Services.IServices;

namespace StudyBuddyWebBlazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

            return Ok(result);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result); 
            }

            return Ok(result);
        }

        [HttpGet("/auth/external-login")]
        public IActionResult ExternalLogin(string returnUrl = "/dashboard")
        {
            var props = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                RedirectUri = returnUrl
            };

            return Challenge(props, Microsoft.AspNetCore.Authentication.Google.GoogleDefaults.AuthenticationScheme);
        }

    }
}
