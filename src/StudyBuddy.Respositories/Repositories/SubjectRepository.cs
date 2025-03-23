using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext context) : base(context) { }
    }
}
