using System;

namespace StudyBuddy.Core.DTOs
{
    public class ReminderDto
    {
        public int? TaskId { get; set; }

        public string? CustomMessage { get; set; }

        public DateTime RemindAt { get; set; }

        public int NotifyMinutesBefore { get; set; } = 60;

        public string UserId { get; set; } = null!;
    }
}
