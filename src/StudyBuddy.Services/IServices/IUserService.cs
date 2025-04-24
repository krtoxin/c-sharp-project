using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudyBuddy.Core.Entities;
using StudyBuddy.Core.DTOs;
using System.Threading.Tasks;

namespace StudyBuddy.Services.IServices
{
    public interface IUserService
    {
        Task<User?> FindByEmailAsync(string email);
        Task<string> GenerateResetCodeAsync(User user);
        Task<ApiResult> ResetPasswordAsync(string email, string code, string newPassword);
    }
}
