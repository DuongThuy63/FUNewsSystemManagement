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
                    .Include(g => g.Tags)
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


                var selectedTagIds = p.Tags.Select(t => t.TagId).ToList();

                var existingTags = context.Tags.Where(t => selectedTagIds.Contains(t.TagId)).ToList();
                var newTags = p.Tags.Where(t => !existingTags.Any(et => et.TagId == t.TagId)).ToList();
                context.Tags.AddRange(newTags);
                context.SaveChanges();

                p.Tags = existingTags.Concat(newTags).ToList();

                context.NewsArticles.Add(p);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                throw;
            }
        }


        public static void UpdateNewsArticle(NewsArticle p)
        {
            try
            {
                using var context = new FunewsManagementContext();

                var existingArticle = context.NewsArticles
                    .Include(a => a.Tags)
                    .FirstOrDefault(a => a.NewsArticleId == p.NewsArticleId);

                if (existingArticle == null)
                {
                    throw new Exception("Không tìm thấy bài viết.");
                }

               
                existingArticle.NewsTitle = p.NewsTitle;
                existingArticle.Headline = p.Headline;
                existingArticle.NewsContent = p.NewsContent;
                existingArticle.NewsSource = p.NewsSource;
                existingArticle.CategoryId = p.CategoryId;
                existingArticle.NewsStatus = p.NewsStatus;
                existingArticle.UpdatedById = p.UpdatedById;
                existingArticle.ModifiedDate = p.ModifiedDate;

                
                if (p.Tags != null)
                {
                    var selectedTagIds = p.Tags.Select(t => t.TagId).ToList();
                    var selectedTags = context.Tags.Where(t => selectedTagIds.Contains(t.TagId)).ToList();

                    existingArticle.Tags.Clear(); 
                    foreach (var tag in selectedTags)
                    {
                        existingArticle.Tags.Add(tag);
                    }
                }

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
                    context.NewsArticles.Include(a => a.Tags)
                                        .FirstOrDefault(a => a.NewsArticleId == p.NewsArticleId);
                p1.Tags.Clear();
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
             .Include(j => j.Tags)
             .FirstOrDefault(c => c.NewsArticleId == id);
        }
    }
}
