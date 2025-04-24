using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class StudyTaskRepository : BaseRepository<StudyTask>, IStudyTaskRepository
    {
        public StudyTaskRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<StudyTask>> GetAllAsync()
        {
            return await _context.StudyTasks
                .Include(t => t.SubTopic)
                .ToListAsync();
        }

        public override async Task<StudyTask?> GetByIdAsync(int id)
        {
            return await _context.StudyTasks
                .Include(t => t.SubTopic)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<StudyTask>> GetBySubTopicIdAsync(int subTopicId)
        {
            return await _context.StudyTasks
                .Where(t => t.SubTopicId == subTopicId)
                .Include(t => t.SubTopic)
                .ToListAsync();
        }
    }
}
