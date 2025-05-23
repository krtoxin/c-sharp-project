using StudyBuddy.Services.IServices;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Forms;

namespace StudyBuddy.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        private static Dictionary<string, string> _resetCodeHashes = new();

        public Task<string> GenerateResetCodeAsync(User user)
        {
            var code = new Random().Next(100000, 999999).ToString();

            var hasher = new PasswordHasher<string>();
            var hashedCode = hasher.HashPassword(user.Email, code);

            _resetCodeHashes[user.Email] = hashedCode;

            return Task.FromResult(code);
        }

        public async Task<ApiResult> ResetPasswordAsync(string email, string code, string newPassword)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return ApiResult.Failure("User not found");

            if (!_resetCodeHashes.ContainsKey(email))
                return ApiResult.Failure("Reset code not found");

            var hasher = new PasswordHasher<string>();
            var result = hasher.VerifyHashedPassword(email, _resetCodeHashes[email], code);

            if (result == PasswordVerificationResult.Failed)
                return ApiResult.Failure("Invalid reset code");

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, newPassword);

            await _userRepository.UpdateAsync(user);
            _resetCodeHashes.Remove(email);

            return ApiResult.Success();
        }
        public async Task CreateUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<ApiResult> SaveAvatarAsync(string userId, IBrowserFile file)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return ApiResult.Failure("User not found");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(stream);

            user.ProfileImage = $"/avatars/{fileName}";
            await _userRepository.UpdateAsync(user);

            return ApiResult.Success();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }


    }
}