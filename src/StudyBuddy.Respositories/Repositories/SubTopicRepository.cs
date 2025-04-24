using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Repositories.Repositories
{
    public class SubTopicRepository : BaseRepository<SubTopic>, ISubTopicRepository
    {
        public SubTopicRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<SubTopic>> GetBySubjectIdAsync(int subjectId)
        {
            return await _context.SubTopics
                .Where(st => st.SubjectId == subjectId)
                .ToListAsync();
        }

        public override async Task<IEnumerable<SubTopic>> GetAllAsync()
        {
            return await _context.SubTopics
                .Include(s => s.Subject)
                .ToListAsync();
        }

        public override async Task<SubTopic?> GetByIdAsync(int id)
        {
            return await _context.SubTopics
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
