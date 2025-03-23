using Microsoft.AspNetCore.Identity;
using StudyBuddy.Core.DTOs;
using StudyBuddy.Core.Entities;

namespace StudyBuddyWebBlazor.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserWithRolesDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var result = new List<UserWithRolesDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserWithRolesDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return result;
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}
