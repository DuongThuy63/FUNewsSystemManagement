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
        private readonly ITagService _contextTag;
        public NewsArticlesController(INewsArticleService contextArticle, ICategoryService contextCategory, ITagService contextTag)
        {
            _contextArticle = contextArticle;
            _contextCategory = contextCategory;
            _contextTag = contextTag;
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

        [Authorize(Roles = "Admin,Lecturer,Staff")]
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
            ViewBag.Tags = _contextTag.GetTags().ToList();
            return View();
        }

        // POST: NewsArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("NewsArticleId, NewsTitle, Headline, NewsContent, NewsSource, CategoryId, NewsStatus, CreatedByID, CreatedDate, ModifiedDate")]
            NewsArticle newsArticle,
            List<int> SelectedTagIds,
            string NewsTag)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin người dùng.");
                return View(newsArticle);
            }

            newsArticle.CreatedById = short.Parse(userIdClaim);
            newsArticle.CreatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
               
                if (!string.IsNullOrEmpty(NewsTag))
                {
                    var tagNames = NewsTag.Split(',')
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrEmpty(t))
                        .Distinct()
                        .ToList();

                    foreach (var tagName in tagNames)
                    {
                        var existingTag = _contextTag.GetTags().FirstOrDefault(t => t.TagName == tagName);
                        if (existingTag == null)
                        {
                            
                            CreateTag(tagName);

                        
                            existingTag = _contextTag.GetTags().FirstOrDefault(t => t.TagName == tagName);
                        }

                        if (existingTag != null)
                        {
                            SelectedTagIds.Add(existingTag.TagId);
                        }
                    }
                }

                
                var selectedTags = _contextTag.GetTags().Where(t => SelectedTagIds.Contains(t.TagId)).ToList();
                 newsArticle.Tags = selectedTags;

                
                _contextArticle.SaveArticle(newsArticle);

                return RedirectToAction(nameof(Index));
            }

       
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            ViewBag.Tags = _contextTag.GetTags().ToList();

            return View(newsArticle);
        }



        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _contextArticle.GetNewsArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            // Danh sách Category
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName", article.CategoryId);
            ViewBag.Tags = _contextTag.GetTags();
            return View(article);
        }


        // POST: NewsArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("NewsArticleId,NewsTitle,Headline,CreatedDate,NewsContent,NewsSource,CategoryId,NewsStatus,CreatedById,UpdatedById,ModifiedDate")] NewsArticle newsArticle, List<int> SelectedTagIds, string? NewTags)
        {
            if (id != newsArticle.NewsArticleId)
            {
                return NotFound();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin người dùng.");
                return View(newsArticle);
            }

            newsArticle.UpdatedById = short.Parse(userIdClaim);
            newsArticle.ModifiedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    // 🔹 Lấy bài viết cũ
                    var existingArticle = _contextArticle.GetNewsArticleById(newsArticle.NewsArticleId);
                    if (existingArticle == null)
                    {
                        return NotFound();
                    }

                    // 🔹 Nếu có nhập Tag mới thì xử lý
                    if (!string.IsNullOrEmpty(NewTags))
                    {
                        var tagNames = NewTags.Split(',')
                            .Select(t => t.Trim())
                            .Where(t => !string.IsNullOrEmpty(t))
                            .ToList();

                        foreach (var tagName in tagNames)
                        {
                            var existingTag = _contextTag.GetTags().FirstOrDefault(t => t.TagName == tagName);
                            if (existingTag == null)
                            {

                                CreateTag(tagName);


                                existingTag = _contextTag.GetTags().FirstOrDefault(t => t.TagName == tagName);
                            }

                            if (existingTag != null)
                            {
                                SelectedTagIds.Add(existingTag.TagId);
                            }
                        }
                    }
                    

                    // 🔹 Cập nhật danh sách Tags cho bài viết
                    var selectedTags = _contextTag.GetTags().Where(t => SelectedTagIds.Contains(t.TagId)).ToList();
                    newsArticle.Tags = selectedTags;

                    // 🔹 Cập nhật bài viết
                    _contextArticle.UpdateArticle(newsArticle);
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

            ViewBag.Tags = _contextTag.GetTags();
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryDesciption", newsArticle.CategoryId);

            return View(newsArticle);
        }


        // GET: NewsArticles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _contextArticle.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            ViewBag.Tags = _contextTag.GetTags();
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            return View(newsArticle);
        }

        // POST: NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var article = _contextArticle.GetNewsArticleById(id);
            if (article != null)
            {
                _contextArticle.DeleteArticle(article);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NewsArticleExists(string id)
        {
            return _context.NewsArticle.Any(e => e.NewsArticleId == id);
        }

        public IActionResult CreateTag(string tagName)
        {
            if (!string.IsNullOrEmpty(tagName))
            {
                var existingTag = _contextTag.GetTags().FirstOrDefault(t => t.TagName == tagName);
                if (existingTag == null)
                {
                    
                    int newTagId = _contextTag.GetTags().Any() ? _contextTag.GetTags().Max(t => t.TagId) + 1 : 1;

                    var newTag = new Tag    
                    {
                        TagId = newTagId, 
                        TagName = tagName
                    };

                    _contextTag.AddTag(newTag);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Staff")]
        public IActionResult SearchByDate(DateTime startDate, DateTime endDate)
        {
            
            if (startDate == default || endDate == default)
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ ngày bắt đầu và ngày kết thúc.");
                return View("Index", new List<NewsArticle>());
            }

            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.ToString("yyyy-MM-dd");

            var articles = _contextArticle.GetArticles()
                .Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate)
                .OrderByDescending(a => a.CreatedDate)
                .ToList();

            return View("Index", articles); // Trả về danh sách bài viết tìm được
        }
        public IActionResult MyArticles()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToAction("Index"); 
            }

            short userId = short.Parse(userIdClaim);

            var myArticles = _contextArticle.GetArticles()
                .Where(a => a.CreatedById == userId)
                .OrderByDescending(a => a.CreatedDate)
                .ToList();

            return View("Index", myArticles); 
        }

    }
}
