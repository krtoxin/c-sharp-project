using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;
using StudyBuddy.Services.IServices;

namespace StudyBuddy.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo) => _repo = repo;
        public async Task<IEnumerable<Category>> GetParentCategoriesAsync()
            => await _repo.GetParentCategoriesAsync();

        public async Task<IEnumerable<Category>> GetSubcategoriesAsync(int parentId)
            => await _repo.GetSubcategoriesAsync(parentId);
    }
}
