using Microsoft.AspNetCore.Http;
using StudyBuddy.Core.Entities;
using StudyBuddy.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using StudyBuddy.Core.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Services.Services
{
    public class TaskAttemptService : ITaskAttemptService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskAttemptService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SaveAttemptAsync(TaskAttempt attempt)
        {
            if (string.IsNullOrEmpty(attempt.UserId))
                throw new InvalidOperationException("User ID must be provided when saving attempt.");

            attempt.AttemptTime = DateTime.UtcNow;

            _context.TaskAttempts.Add(attempt);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TaskAttempt>> GetAttemptsByTaskIdAsync(int taskId)
        {
            return await _context.TaskAttempts
                .Include(a => a.User)
                .Where(a => a.TaskId == taskId)
                .OrderByDescending(a => a.AttemptTime)
                .ToListAsync();
        }

        public async Task<List<TaskAttempt>> GetAttemptsByUserAndTaskIdAsync(string userId, int taskId)
        {
            return await _context.TaskAttempts
                .Include(a => a.User)
                .Where(a => a.TaskId == taskId && a.UserId == userId)
                .OrderByDescending(a => a.AttemptTime)
                .ToListAsync();
        }

    }
}