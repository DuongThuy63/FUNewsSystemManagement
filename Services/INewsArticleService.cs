using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface INewsArticleService
    {
        void SaveArticle(NewsArticle p);
        void DeleteArticle(NewsArticle p);
        void UpdateArticle(NewsArticle p);
        List<NewsArticle> GetArticles();
        NewsArticle GetNewsArticleById(string id);
    }
}
