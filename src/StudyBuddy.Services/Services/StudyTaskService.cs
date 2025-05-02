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
        private readonly ISubTopicRepository _subTopicRepo;

        public StudyTaskService(
            IStudyTaskRepository taskRepo,
            ITaskOptionRepository optionRepo,
            ISubTopicRepository subTopicRepo)
        {
            _taskRepo = taskRepo;
            _optionRepo = optionRepo;
            _subTopicRepo = subTopicRepo;
        }

        public async Task<IEnumerable<StudyTask>> GetAllAsync()
        {
            var tasks = await _taskRepo.GetAllAsync();

            foreach (var task in tasks)
            {
                if (task.SubTopic == null)
                {
                    task.SubTopic = await _subTopicRepo.GetByIdAsync(task.SubTopicId);
                }
            }

            return tasks;
        }

        public async Task<StudyTask?> GetByIdAsync(int id)
        {
            var task = await _taskRepo.GetByIdAsync(id);

            if (task?.SubTopic == null)
            {
                task!.SubTopic = await _subTopicRepo.GetByIdAsync(task.SubTopicId);
            }

            return task;
        }

        public Task<IEnumerable<StudyTask>> GetBySubTopicIdAsync(int subTopicId)
            => _taskRepo.GetBySubTopicIdAsync(subTopicId);

        public async Task<List<SubTopic>> GetAllSubTopicsAsync()
            => (await _subTopicRepo.GetAllAsync()).ToList();

        public async Task CreateAsync(StudyTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Question))
                throw new ArgumentException("Question is required.");
            if (task.SubTopicId <= 0)
                throw new ArgumentException("SubTopic must be selected.");

            task.SubTopic = await _subTopicRepo.GetByIdAsync(task.SubTopicId);

            await _taskRepo.AddAsync(task);
            await _taskRepo.SaveAsync();

            if (task.TaskType == TaskType.MultipleChoice && task.Options?.Any() == true)
            {
                foreach (var option in task.Options)
                    option.StudyTaskId = task.Id;

                await _optionRepo.AddRangeAsync(task.Options);
                await _optionRepo.SaveAsync();
            }
        }

        public async Task UpdateAsync(StudyTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Question))
                throw new ArgumentException("Question is required.");
            if (task.SubTopicId <= 0)
                throw new ArgumentException("SubTopic must be selected.");

            await _taskRepo.UpdateAsync(task);
            await _taskRepo.SaveAsync();

            await _optionRepo.DeleteByTaskIdAsync(task.Id);

            if (task.TaskType == TaskType.MultipleChoice && task.Options?.Any() == true)
            {
                foreach (var option in task.Options)
                    option.StudyTaskId = task.Id;

                await _optionRepo.AddRangeAsync(task.Options);
                await _optionRepo.SaveAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _taskRepo.GetByIdAsync(id);
            if (existing == null) return;

            await _optionRepo.DeleteByTaskIdAsync(id);
            await _taskRepo.DeleteAsync(existing);
            await _taskRepo.SaveAsync();
        }
    }
}
