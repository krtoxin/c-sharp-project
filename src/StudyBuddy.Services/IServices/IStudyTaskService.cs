using StudyBuddy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Services.IServices
{
    public interface IStudyTaskService
    {
        Task<IEnumerable<StudyTask>> GetAllAsync();
        Task<StudyTask?> GetByIdAsync(int id);
        Task<IEnumerable<StudyTask>> GetBySubTopicIdAsync(int subTopicId);
        Task CreateAsync(StudyTask task);
        Task UpdateAsync(StudyTask task);
        Task DeleteAsync(int id);
        Task<List<SubTopic>> GetAllSubTopicsAsync();
        Task<List<SubTopic>> GetSubTopicsBySubjectIdAsync(int subjectId);
        Task<IEnumerable<StudyTask>> GetAllTasksAsync();


    }
}
