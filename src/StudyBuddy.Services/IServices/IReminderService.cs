using StudyBuddy.Core.DTOs;
using StudyBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Services.IServices
{
    public interface IReminderService
    {
        Task CreateReminderAsync(ReminderDto dto);
        Task<List<Reminder>> GetUpcomingRemindersAsync(string userId);
    }
}
