using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.Core.Entities;

namespace StudyBuddy.Services.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetParentCategoriesAsync();
        Task<IEnumerable<Category>> GetSubcategoriesAsync(int parentId);
    }
}