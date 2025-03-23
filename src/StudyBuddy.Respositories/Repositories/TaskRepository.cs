using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class TaskRepository : BaseRepository<StudyTask>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context) { }
    }
}
