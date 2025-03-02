using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Category> GetCategories() => CategoryDAO.GetCategories();

        public void DeleteCategory(Category p) => CategoryDAO.DeleteCategory(p);
        public void SaveCategory(Category p) => CategoryDAO.CreateCategory(p);

        public void UpdateCategory(Category p) => CategoryDAO.UpdateCategory(p);
        
        public Category GetCategoryById(short id) => CategoryDAO.GetCategoryById(id);
    }
}
