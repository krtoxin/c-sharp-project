using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Core.DTOs;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Services.IServices
{
    public interface IStudyTaskService
    {
        Task<IEnumerable<StudyTask>> GetAllAsync();
        Task<StudyTask?> GetByIdAsync(int id);
        Task<IEnumerable<StudyTask>> GetBySubTopicIdAsync(int subTopicId);
        Task CreateAsync(StudyTask task);
        Task UpdateAsync(StudyTask task);
        Task<ApiResult> DeleteAsync(int id);
        Task<List<SubTopic>> GetAllSubTopicsAsync();
        Task<List<SubTopic>> GetSubTopicsBySubjectIdAsync(int subjectId);
        Task<IEnumerable<StudyTask>> GetAllTasksAsync();


    }
}
