using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();

        void SaveCategory(Category p);
        void DeleteCategory(Category p);
        void UpdateCategory(Category p);
        
        Category GetCategoryById(short id);
    }
}
