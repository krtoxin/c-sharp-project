using StudyBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Services.IServices
{
    public interface ITaskAttemptService
    {
        Task SaveAttemptAsync(TaskAttempt attempt);
        Task<List<TaskAttempt>> GetAttemptsByTaskIdAsync(int taskId);
    }
}
