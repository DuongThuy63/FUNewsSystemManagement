using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class CategoryDAO
    {
        public static List<Category> GetCategories()
        {
            var listCategories = new List<Category>();
            try
            {
                using var context = new FunewsManagementContext();
                listCategories = context.Categories.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listCategories;
        }


        public static void CreateCategory(Category p)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.Categories.Add(p);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateCategory(Category p)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.Entry<Category>(p).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void DeleteCategory(Category p) { }


        public static Category GetCategoryById(short id)
        {
            using var db = new FunewsManagementContext();
            return db.Categories.FirstOrDefault(c => c.CategoryId == id);
        }
    }
}
