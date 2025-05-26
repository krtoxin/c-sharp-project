using StudyBuddy.Core.Data;
using StudyBuddy.Core.DTOs;
using StudyBuddy.Core.Entities;
using StudyBuddy.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace StudyBuddy.Services.Services
{
    public class ReminderService : IReminderService
    {
        private readonly AppDbContext _context;

        public ReminderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateReminderAsync(ReminderDto dto)
        {
            var reminder = new Reminder
            {
                UserId = dto.UserId,
                TaskId = dto.TaskId,
                CustomMessage = dto.CustomMessage,
                RemindAt = dto.RemindAt.Kind == DateTimeKind.Utc
                            ? dto.RemindAt
                            : dto.RemindAt.ToUniversalTime(),
                NotifyMinutesBefore = dto.NotifyMinutesBefore,
                IsSent = false
            };

            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Reminder>> GetUpcomingRemindersAsync(string userId)
        {
            return await _context.Reminders
                .Where(r => r.UserId == userId && !r.IsSent && r.RemindAt <= DateTime.UtcNow)
                .OrderBy(r => r.RemindAt)
                .ToListAsync();
        }
    }

}
