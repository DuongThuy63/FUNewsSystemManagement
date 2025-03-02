using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using FUNewsManagementSystem.Data;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Lecturer,Staff")]
    public class NewsArticlesController : Controller
    {
        private readonly FUNewsManagementSystemContext _context;
        private readonly INewsArticleService _contextArticle;
        private readonly ICategoryService _contextCategory;
        private readonly IAccountService _contextAccount;
        public NewsArticlesController(INewsArticleService contextArticle, ICategoryService contextCategory)
        {
            _contextArticle = contextArticle;
            _contextCategory = contextCategory;
        }

        [Authorize(Roles = "Admin,Lecturer,Staff")]
        // GET: NewsArticles
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "SystemAccounts");
            }
                var articles = _contextArticle.GetArticles();
                return View(articles.ToList());
            
        }

        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _contextArticle.GetNewsArticleById((string)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);

        }

        // GET: NewsArticles/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryDesciption");
            
            return View();
        }

        // POST: NewsArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsArticleId, NewsTitle,Headline,NewsContent,NewsSource,CategoryId,NewsStatus,CreatedByID,CreatedDate,ModifiedDate")] NewsArticle newsArticle)
        {
            newsArticle.CreatedById = 3;
            newsArticle.CreatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                _contextArticle.SaveArticle(newsArticle);

                return RedirectToAction(nameof(Index));
            }


            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            return View(newsArticle);
        }


        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticle.FindAsync(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            ViewData["CreatedById"] = new SelectList(_context.SystemAccount, "AccountId", "AccountId", newsArticle.CreatedById);
            return View(newsArticle);
        }

        // POST: NewsArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NewsArticleId,NewsTitle,Headline,CreatedDate,NewsContent,NewsSource,CategoryId,NewsStatus,CreatedById,UpdatedById,ModifiedDate")] NewsArticle newsArticle)
        {
            if (id != newsArticle.NewsArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsArticleExists(newsArticle.NewsArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            ViewData["CreatedById"] = new SelectList(_context.SystemAccount, "AccountId", "AccountId", newsArticle.CreatedById);
            return View(newsArticle);
        }

        // GET: NewsArticles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticle
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(m => m.NewsArticleId == id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // POST: NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var newsArticle = await _context.NewsArticle.FindAsync(id);
            if (newsArticle != null)
            {
                _context.NewsArticle.Remove(newsArticle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsArticleExists(string id)
        {
            return _context.NewsArticle.Any(e => e.NewsArticleId == id);
        }
    }
}
