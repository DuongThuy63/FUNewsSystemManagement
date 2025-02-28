using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NewsArticleService : INewsArticleService
    {

        private readonly INewsAticleRepository iNewsArticleRepository;
        public NewsArticleService()
        {
            iNewsArticleRepository = new NewsArticleRepository();

        }

        public void DeleteArticle(NewsArticle p)
        {
            iNewsArticleRepository.DeleteArticle(p);
        }

        public NewsArticle GetNewsArticleById(string id)
        {
            return iNewsArticleRepository.GetNewsArticleById(id);
        }

        public List<NewsArticle> GetArticles()
        {
            return iNewsArticleRepository.GetArticles();
        }

        public void SaveArticle(NewsArticle p)
        {
            iNewsArticleRepository.SaveArticle(p);
        }

        public void UpdateArticle(NewsArticle p)
        {
            iNewsArticleRepository.UpdateArticle(p);
        }

    }
}
