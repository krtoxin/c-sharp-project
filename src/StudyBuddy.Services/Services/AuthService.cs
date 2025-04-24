using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using StudyBuddy.Core.DTOs;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Services.IServices;

namespace StudyBuddy.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            IConfiguration config,
            IJwtTokenGenerator tokenGenerator,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _config = config;
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto dto)
        {
            var existingByEmail = await _userRepository.GetByEmailAsync(dto.Email);
            var existingByUsername = await _userRepository.FindByNameAsync(dto.UserName);

            if (existingByEmail != null || existingByUsername.Any())
            {
                var errors = new List<string>();
                if (existingByEmail != null) errors.Add("Email is already in use.");
                if (existingByUsername.Any()) errors.Add("Username is already taken.");

                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = errors
                };
            }

            var hashedPassword = _passwordHasher.HashPassword(null!, dto.Password);

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = dto.UserName,
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                RoleId = 3 
            };

            await _userRepository.AddAsync(user);

            return await GenerateAuthResult(user);
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = (await _userRepository.FindByNameAsync(dto.Identifier)).FirstOrDefault()
                       ?? await _userRepository.GetByEmailAsync(dto.Identifier);

            if (user == null)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new List<string> { "User not found" }
                };
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Invalid password" }
                };
            }

            return await GenerateAuthResult(user);
        }

        public async Task<User?> GetUserByLogin(string identifier, string password)
        {
            var user = (await _userRepository.FindByNameAsync(identifier)).FirstOrDefault()
                       ?? await _userRepository.GetByEmailAsync(identifier);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _refreshTokenRepository.RemoveByUserIdAsync(refreshToken.UserId);
            await _refreshTokenRepository.AddAsync(refreshToken);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _refreshTokenRepository.GetByTokenAsync(token);
        }

        private async Task<AuthResultDto> GenerateAuthResult(User user)
        {
            var roles = new List<string> { user.Role?.Name ?? "User" };

            var accessToken = _tokenGenerator.GenerateToken(user, roles);
            var refreshToken = _tokenGenerator.GenerateRefreshToken(user);

            await AddRefreshTokenAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            });

            return new AuthResultDto
            {
                IsSuccess = true,
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpired = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds()
            };
        }
    }
}
