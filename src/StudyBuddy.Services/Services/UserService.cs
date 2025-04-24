using StudyBuddy.Services.IServices;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Core.DTOs;
using Microsoft.AspNetCore.Identity;

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

        public async Task<string> GenerateResetCodeAsync(User user)
        {
            var code = new Random().Next(100000, 999999).ToString();

            var hasher = new PasswordHasher<string>();
            var hashedCode = hasher.HashPassword(user.Email, code);

            _resetCodeHashes[user.Email] = hashedCode;

            return code;
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
    }
}