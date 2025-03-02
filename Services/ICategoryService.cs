using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
        void CreateCategory(Category p);
        void DeleteCategory(Category p);
        void UpdateCategory(Category p);

        Category GetCategoryById(short id);
    }
}
