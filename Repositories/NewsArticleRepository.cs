using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects;

namespace Repositories
{
    public class NewsArticleRepository : INewsAticleRepository
    {
        public void DeleteArticle(NewsArticle p) => NewsArticleDAO.DeleteNewsArticle(p);
        public void SaveArticle(NewsArticle p) => NewsArticleDAO.SaveArticle(p);

        public void UpdateArticle(NewsArticle p) => NewsArticleDAO.UpdateNewsArticle(p);
        public List<NewsArticle> GetArticles() => NewsArticleDAO.GetArticles();
        public NewsArticle GetNewsArticleById(string id) => NewsArticleDAO.GetNewArticleById(id);

        
    }
}
