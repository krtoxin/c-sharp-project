using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Services.Services
{
    public class SubTopicService : ISubTopicService
    {
        private readonly ISubTopicRepository _subTopicRepo;

        public SubTopicService(ISubTopicRepository subTopicRepo)
        {
            _subTopicRepo = subTopicRepo;
        }

        public async Task<IEnumerable<SubTopic>> GetAllAsync()
            => await _subTopicRepo.GetAllAsync();

        public async Task<SubTopic?> GetByIdAsync(int id)
            => await _subTopicRepo.GetByIdAsync(id);

        public async Task<IEnumerable<SubTopic>> GetBySubjectIdAsync(int subjectId)
            => await _subTopicRepo.GetBySubjectIdAsync(subjectId);

        public async Task CreateAsync(SubTopic subTopic)
        {
            if (string.IsNullOrWhiteSpace(subTopic.Title))
                throw new ArgumentException("Title is required.");

            await _subTopicRepo.AddAsync(subTopic);
        }

        public async Task UpdateAsync(SubTopic subTopic)
        {
            var existing = await _subTopicRepo.GetByIdAsync(subTopic.Id);
            if (existing == null)
                throw new Exception("SubTopic not found");

            existing.Title = subTopic.Title;
            existing.Description = subTopic.Description;
            existing.SubjectId = subTopic.SubjectId;

            await _subTopicRepo.UpdateAsync(existing);
        }


        public async Task DeleteAsync(int id)
        {
            var existing = await _subTopicRepo.GetByIdAsync(id);
            if (existing is not null)
                await _subTopicRepo.DeleteAsync(existing);
        }
    }
}
