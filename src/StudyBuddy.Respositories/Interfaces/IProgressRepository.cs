using StudyBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Repositories.Interfaces
{
    public interface IProgressRepository : IBaseRepository<UserProgress>
    {
        Task<int> GetCompletedTasksCountAsync(string userId, int subtopicId);
        Task ResetProgressAsync(string userId, int subtopicId);
    }
}
