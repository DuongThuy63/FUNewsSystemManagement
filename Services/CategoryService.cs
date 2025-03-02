using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository iCategoryRepository;

        public CategoryService()
        {
            iCategoryRepository = new CategoryRepository();
        }

        public List<Category> GetCategories()
        {
            return iCategoryRepository.GetCategories();
        }

        public void DeleteCategory(Category p)
        {
            iCategoryRepository.DeleteCategory(p);
        }

        public Category GetCategoryById(short id)
        {
            return iCategoryRepository.GetCategoryById(id);
        }

        
        public void CreateCategory(Category p)
        {
            iCategoryRepository.SaveCategory(p);
        }

        public void UpdateCategory(Category p)
        {
            iCategoryRepository.UpdateCategory(p);
        }

    }
}
