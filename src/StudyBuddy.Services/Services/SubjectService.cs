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
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepo;

        public SubjectService(ISubjectRepository subjectRepo)
        {
            _subjectRepo = subjectRepo;
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _subjectRepo.GetAllAsync();
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await _subjectRepo.GetByIdAsync(id);
        }

        public async Task<Subject> CreateAsync(Subject subject)
        {
            var allSubjects = await _subjectRepo.GetAllAsync();
            if (allSubjects.Any(s => s.Name.ToLower() == subject.Name.ToLower()))
                throw new Exception("Subject with the same name already exists.");

            if (string.IsNullOrWhiteSpace(subject.Name))
                throw new Exception("Subject name is required.");

            await _subjectRepo.AddAsync(subject);
            return subject;
        }

        public async Task<bool> UpdateAsync(int id, Subject updated)
        {
            var existing = await _subjectRepo.GetByIdAsync(id);
            if (existing == null)
                return false;

            if (string.IsNullOrWhiteSpace(updated.Name))
                throw new Exception("Subject name cannot be empty.");

            existing.Name = updated.Name;
            existing.Icon = updated.Icon;
            existing.CategoryId = updated.CategoryId;

            await _subjectRepo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _subjectRepo.GetByIdAsync(id);
            if (existing == null)
                return false;

            await _subjectRepo.DeleteAsync(existing);
            return true;
        }
    }
}