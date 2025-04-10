using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyBuddy.Core.Entities;
using StudyBuddy.Core.Enums;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Services.IServices;

namespace StudyBuddy.Services.Services
{
    public class StudyTaskService : IStudyTaskService
    {
        private readonly IStudyTaskRepository _taskRepo;
        private readonly ITaskOptionRepository _optionRepo;

        public StudyTaskService(
            IStudyTaskRepository taskRepo,
            ITaskOptionRepository optionRepo)
        {
            _taskRepo = taskRepo;
            _optionRepo = optionRepo;
        }

        public async Task<IEnumerable<StudyTask>> GetAllAsync()
            => await _taskRepo.GetAllAsync();

        public async Task<StudyTask?> GetByIdAsync(int id)
            => await _taskRepo.GetByIdAsync(id);

        public async Task<IEnumerable<StudyTask>> GetBySubTopicIdAsync(int subTopicId)
            => await _taskRepo.GetBySubTopicIdAsync(subTopicId);

        public async Task CreateAsync(StudyTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Question))
                throw new ArgumentException("Question is required.");

            await _taskRepo.AddAsync(task);

            if (task.TaskType == TaskType.MultipleChoice && task.Options?.Any() == true)
            {
                foreach (var option in task.Options)
                {
                    option.StudyTaskId = task.Id;
                    await _optionRepo.AddAsync(option);
                }
            }
        }

        public async Task UpdateAsync(StudyTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Question))
                throw new ArgumentException("Question is required.");

            await _taskRepo.UpdateAsync(task);

            if (task.TaskType == TaskType.MultipleChoice)
            {
                await _optionRepo.DeleteByTaskIdAsync(task.Id);

                if (task.Options?.Any() == true)
                {
                    foreach (var option in task.Options)
                    {
                        option.StudyTaskId = task.Id;
                        await _optionRepo.AddAsync(option);
                    }
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _taskRepo.GetByIdAsync(id);
            if (existing != null)
            {
                await _taskRepo.DeleteAsync(existing);
                await _optionRepo.DeleteByTaskIdAsync(id);
            }
        }
    }
}
