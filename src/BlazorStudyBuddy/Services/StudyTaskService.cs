using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Entities;
using StudyBuddy.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyBuddyWebBlazor.Services
{
    public class StudyTaskService
    {
        private readonly AppDbContext _context;

        public StudyTaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudyTask>> GetAllTasksAsync()
        {
            return await _context.StudyTasks
                .Include(t => t.SubTopic)
                .ToListAsync();
        }

        public async Task<List<SubTopic>> GetAllSubTopicsAsync()
        {
            return await _context.SubTopics.ToListAsync();
        }
        public async Task<List<StudyTask>> GetTasksBySubtopicAsync(int subTopicId)
        {
            return await _context.StudyTasks
                .Where(t => t.SubTopicId == subTopicId)
                .ToListAsync();
        }

        public async Task<StudyTask> GetTaskByIdAsync(int id)
        {
            return await _context.StudyTasks
                .Include(t => t.SubTopic)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTaskAsync(StudyTask task)
        {
            _context.StudyTasks.Add(task);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateTaskAsync(StudyTask task)
        {
            _context.StudyTasks.Update(task);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.StudyTasks.FindAsync(id);
            if (task != null)
            {
                _context.StudyTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
