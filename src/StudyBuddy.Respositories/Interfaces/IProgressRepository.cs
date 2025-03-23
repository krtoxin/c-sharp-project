using StudyBuddy.Core.Entities;

namespace StudyBuddy.Repositories.Interfaces
{
    public interface IProgressRepository : IBaseRepository<UserProgress>
    {
        Task<int> GetCompletedTasksCountAsync(string userId, int subtopicId);
        Task ResetProgressAsync(string userId, int subtopicId);
    }
}
