using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
