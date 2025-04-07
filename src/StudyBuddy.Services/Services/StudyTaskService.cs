using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Services.IServices;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Services.Services
{
    public class StudyTaskService : IStudyTaskService
    {
        private readonly IStudyTaskRepository _taskRepo;

        public StudyTaskService(IStudyTaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<IEnumerable<StudyTask>> GetAllAsync()
            => await _taskRepo.GetAllAsync();

        public async Task<StudyTask?> GetByIdAsync(int id)
            => await _taskRepo.GetByIdAsync(id);

        public async Task<IEnumerable<StudyTask>> GetBySubTopicIdAsync(int subTopicId)
            => await _taskRepo.GetBySubTopicIdAsync(subTopicId);

        public async Task CreateAsync(StudyTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Question) || string.IsNullOrWhiteSpace(task.CorrectAnswer))
                throw new ArgumentException("Question and CorrectAnswer are required.");

            await _taskRepo.AddAsync(task);
        }

        public async Task UpdateAsync(StudyTask task)
        {
            await _taskRepo.UpdateAsync(task);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _taskRepo.GetByIdAsync(id);
            if (existing is not null)
                await _taskRepo.DeleteAsync(existing);
        }
    }
}
