using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class TaskOptionRepository : BaseRepository<TaskOption>, ITaskOptionRepository
    {
        public TaskOptionRepository(AppDbContext context) : base(context) { }

        public async Task DeleteByTaskIdAsync(int taskId)
        {
            var options = await _context.TaskOptions
                .Where(o => o.StudyTaskId == taskId)
                .ToListAsync();

            _context.TaskOptions.RemoveRange(options);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskOption>> GetByTaskIdAsync(int taskId)
        {
            return await _context.TaskOptions
                .Where(o => o.StudyTaskId == taskId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TaskOption> options)
        {
            await _context.TaskOptions.AddRangeAsync(options);
        }
    }
}
