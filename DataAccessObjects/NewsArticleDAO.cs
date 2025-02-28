using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class NewsArticleDAO
    {
        private static readonly FunewsManagementContext _context = new FunewsManagementContext();


        public static List<NewsArticle> GetArticles()
        {
            var listArticles = new List<NewsArticle>();
            try
            {
                using var db = new FunewsManagementContext();
                listArticles = db.NewsArticles.Include(f => f.Category)
                    /*.Include(f => f.CreatedById)*/
                    .ToList();
            }
            catch (Exception e) { }
            return listArticles;
        }

        public static void SaveArticle(NewsArticle p)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.NewsArticles.Add(p);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public static void UpdateNewsArticle(NewsArticle p)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.Entry<NewsArticle>(p).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void DeleteNewsArticle(NewsArticle p)
        {
            try
            {
                using var context = new FunewsManagementContext();
                var p1 =
                    context.NewsArticles.SingleOrDefault(c => c.NewsArticleId == p.NewsArticleId);
                context.NewsArticles.Remove(p1);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static NewsArticle GetNewArticleById(string id)
        {
            using var db = new FunewsManagementContext();
            return db.NewsArticles
             .Include(p => p.Category)
             .FirstOrDefault(c => c.NewsArticleId == id);
        }
    }
}
