using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddyWebBlazor.Services
{
    public class SubjectService
    {
        private readonly ISubjectRepository _repository;

        public SubjectService(ISubjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync() => await _repository.GetAllAsync();

        public async Task AddSubjectAsync(Subject subject)
        {
            await _repository.AddAsync(subject);
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            await _repository.UpdateAsync(subject);
        }

        public async Task DeleteSubjectAsync(int id)
        {
            var subject = await _repository.GetByIdAsync(id);
            if (subject != null)
                await _repository.DeleteAsync(subject);
        }
    }
}
