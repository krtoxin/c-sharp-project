using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Respositories.Interfaces
{
    public interface ITaskRepository : IBaseRepository<StudyTask>
    {
        Task<IEnumerable<StudyTask>> GetTasksBySubtopicAsync(int subtopicId);
    }
}
