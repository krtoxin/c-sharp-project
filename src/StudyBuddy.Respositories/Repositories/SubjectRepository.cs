using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Subject> _dbSet;

        public SubjectRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Subject>();
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<Subject?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task AddAsync(Subject subject)
        {
            await _dbSet.AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subject subject)
        {
            _dbSet.Update(subject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Subject subject)
        {
            _dbSet.Remove(subject);
            await _context.SaveChangesAsync();
        }
    }
}
