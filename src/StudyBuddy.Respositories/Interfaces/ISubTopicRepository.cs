using StudyBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Repositories.Interfaces
{
    public interface ISubTopicRepository : IBaseRepository<SubTopic>
    {
        Task<IEnumerable<SubTopic>> GetBySubjectIdAsync(int subjectId);
    }
}
