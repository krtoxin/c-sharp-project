using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Core.DTOs;
using StudyBuddy.Core.Entities;
using StudyBuddy.Services.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using StudyBuddyWebBlazor.Services;

namespace StudyBuddyWebBlazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;


        public AuthController(
        IConfiguration configuration,
        IAuthService authService,
        IJwtTokenGenerator jwtTokenGenerator,
        IUserService userService,
        IEmailService emailService)
        {
            _configuration = configuration;
            _authService = authService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userService = userService;
            _emailService = emailService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginDto dto)
        {
            var user = await _authService.GetUserByLogin(dto.Identifier, dto.Password);
            if (user == null)
                return Unauthorized(new AuthResultDto
                {
                    Token = string.Empty,
                    RefreshToken = string.Empty,
                    TokenExpired = 0,
                    Errors = new() { "Invalid credentials" }
                });

            var roles = new List<string> { user.Role?.Name ?? "User" };

            var accessToken = _jwtTokenGenerator.GenerateToken(user, roles);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

            await _authService.AddRefreshTokenAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            });

            return Ok(new AuthResultDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpired = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds()
            });
        }




        [HttpGet("login-by-refresh")]
        public async Task<ActionResult<AuthResultDto>> LoginByRefreshToken([FromQuery] string refreshToken)
        {
            var storedToken = await _authService.GetRefreshTokenAsync(refreshToken);
            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
                return Unauthorized("Refresh token is invalid or expired");

            var user = storedToken.User;
            var roles = new List<string> { user.Role?.Name ?? "User" };
            var newAccessToken = _jwtTokenGenerator.GenerateToken(user, roles);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

            await _authService.AddRefreshTokenAsync(new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            });

            return Ok(new AuthResultDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                TokenExpired = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds()
            });
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

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpPost("send-reset-code")]
        public async Task<IActionResult> SendResetCode([FromBody] EmailDto request)
        {
            var user = await _userService.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(ApiResult.Failure("User not found"));

            var code = await _userService.GenerateResetCodeAsync(user);
            await _emailService.SendAsync(user.Email, "Your Reset Code", $"Your reset code is: {code}");

            return Ok(ApiResult.Success());
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var result = await _userService.ResetPasswordAsync(model.Email, model.Code, model.NewPassword);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

    }
}
