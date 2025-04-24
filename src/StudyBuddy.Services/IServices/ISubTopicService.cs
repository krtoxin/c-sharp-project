using StudyBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Services.IServices
{
    public interface ISubTopicService
    {
        Task<IEnumerable<SubTopic>> GetAllAsync();
        Task<SubTopic?> GetByIdAsync(int id);
        Task<IEnumerable<SubTopic>> GetBySubjectIdAsync(int subjectId);
        Task CreateAsync(SubTopic subTopic);
        Task UpdateAsync(SubTopic subTopic);
        Task DeleteAsync(int id);
    }
}
