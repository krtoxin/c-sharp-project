using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudyBuddy.Core.Entities;
using StudyBuddy.Core.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace StudyBuddy.Services.IServices
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(string userId);
        Task<User?> FindByEmailAsync(string email);
        Task<string> GenerateResetCodeAsync(User user);
        Task<ApiResult> ResetPasswordAsync(string email, string code, string newPassword);
        Task CreateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task UpdateAsync(User user);
        Task<ApiResult> SaveAvatarAsync(string userId, IBrowserFile file);
    }
}
