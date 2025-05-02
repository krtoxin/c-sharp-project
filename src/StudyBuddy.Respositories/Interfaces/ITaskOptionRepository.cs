using StudyBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Repositories.Interfaces
{
    public interface ITaskOptionRepository : IBaseRepository<TaskOption>
    {
        Task DeleteByTaskIdAsync(int taskId);
        Task<IEnumerable<TaskOption>> GetByTaskIdAsync(int taskId);
        Task AddRangeAsync(IEnumerable<TaskOption> options); // ✅ ADD THIS

    }

}
