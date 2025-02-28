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

namespace FUNewsManagementSystem.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly FUNewsManagementSystemContext _context;
        private readonly INewsArticleService _contextArticle;
        private readonly ICategoryService _contextCategory;
        public ArticlesController(INewsArticleService contextArticle, ICategoryService contextCategory)
        {
            _contextArticle = contextArticle;
            _contextCategory = contextCategory;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                // Redirect to the login page or display an error message
                return RedirectToAction("Login", "SystemAccounts");
            }
            var myStoreContext = _contextArticle.GetArticles();
            return View(myStoreContext.ToList());
        }

        // GET: Articles/Details/5
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

        
        private bool NewsArticleExists(string id)
        {
            return _context.NewsArticle.Any(e => e.NewsArticleId == id);
        }
    }
}
