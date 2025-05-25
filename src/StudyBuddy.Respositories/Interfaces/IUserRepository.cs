using StudyBuddy.Core.Entities;

namespace StudyBuddy.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> FindByNameAsync(string username);
        Task<IEnumerable<User>> GetAllWithRolesAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> ExistsAsync(string userId);
        Task<IEnumerable<User>> GetAllAsync();

    }

}
