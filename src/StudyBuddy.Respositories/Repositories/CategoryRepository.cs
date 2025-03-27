using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Core.Data;
using StudyBuddy.Core.Entities;
using StudyBuddy.Repositories.Interfaces;

namespace StudyBuddy.Repositories.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetParentCategoriesAsync()
        {
            return await _dbSet.Where(c => c.ParentCategoryId == null).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubcategoriesAsync(int parentId)
        {
            return await _dbSet.Where(c => c.ParentCategoryId == parentId).ToListAsync();
        }
    }
}
