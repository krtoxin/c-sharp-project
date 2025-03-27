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
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            IConfiguration config,
            IJwtTokenGenerator tokenGenerator,
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _config = config;
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
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
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.UserName,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            return await GenerateAuthResult(user);
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.FindByNameAsync(dto.Identifier);
            var found = user.FirstOrDefault();

            if (found == null)
            {
                found = await _userRepository.GetByEmailAsync(dto.Identifier);
            }

            if (found == null)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new List<string> { "User not found" }
                };
            }

            var result = _passwordHasher.VerifyHashedPassword(found, found.PasswordHash, dto.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Invalid password" }
                };
            }

            return new AuthResultDto
            {
                IsSuccess = true,
                Token = _tokenGenerator.GenerateToken(found, new List<string>())
            };
        }


        private Task<AuthResultDto> GenerateAuthResult(User user)
        {
            var token = _tokenGenerator.GenerateToken(user, new List<string>());

            return Task.FromResult(new AuthResultDto
            {
                IsSuccess = true,
                Token = token
            });
        }
    }
}
