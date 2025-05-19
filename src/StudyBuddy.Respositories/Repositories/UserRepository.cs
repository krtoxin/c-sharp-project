using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Users;
        }

        public async Task<User?> GetByIdAsync(string id)
            => await _dbSet.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmailAsync(string email)
            => await _dbSet.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<User>> FindByNameAsync(string username)
            => await _dbSet.Include(u => u.Role).Where(u => u.UserName == username).ToListAsync();

        public async Task<IEnumerable<User>> GetAllWithRolesAsync()
            => await _dbSet.Include(u => u.Role).ToListAsync();

        public async Task AddAsync(User user)
        {
            await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _dbSet.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _dbSet.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(string userId)
        {
            return await _dbSet.AnyAsync(u => u.Id == userId);
        }

    }
}
