using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StudyBuddy.Core.Entities
{
    public class Reminder
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public int? TaskId { get; set; }
        public StudyTask? Task { get; set; }

        public string? CustomMessage { get; set; }
        public DateTime RemindAt { get; set; }
        public int NotifyMinutesBefore { get; set; } = 60;
        public bool IsSent { get; set; } = false;
    }
}
