using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<User>> FindByNameAsync(string name)
            => await _dbSet.Where(u => u.FullName.Contains(name)).ToListAsync();
    }
}
