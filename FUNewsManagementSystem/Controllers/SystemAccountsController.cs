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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Configuration;

namespace FUNewsManagementSystem.Controllers
{
    public class SystemAccountsController : Controller
    {
        private readonly FUNewsManagementSystemContext _context;
        private readonly IAccountService _contextAccount;
        private readonly IConfiguration _configuration;
        public SystemAccountsController(IAccountService contextAccount, IConfiguration configuration)
        {
            _contextAccount = contextAccount;
            _configuration = configuration;

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Login(SystemAccount model)
        {/*
            if (ModelState.IsValid)
            {
                var user = _contextAccount.GetAccountByEmail(model.AccountEmail);

                if (user != null && user.AccountPassword == model.AccountPassword)
                {

                    // Lưu thông tin đăng nhập vào session
                    HttpContext.Session.SetString("UserId", user.AccountId.ToString());
                    HttpContext.Session.SetString("Username", user.AccountName);
                    //HttpContext.Session.SetString("UserRole", user.AccountRole?.ToString() ?? "0");
                    HttpContext.Session.SetInt32("UserRole", user.AccountRole ?? 0);
                    Console.WriteLine("UserRole in Session: " + HttpContext.Session.GetInt32("UserRole"));
                    Console.WriteLine("UserRole Set: " + user.AccountRole);
                    // Kiểm tra quyền (Role)
                    if (user.AccountRole == 2)
                    {
                        return RedirectToAction("Index", "Articles"); 
                    }
                    else if (user.AccountRole == 3)
                    {
                        return RedirectToAction("Dashboard", "Admin");; 
                    }
                   
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model);*/


            if (!ModelState.IsValid)
            {
                return View(model);


            }

            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];
            if (model.AccountEmail == adminEmail && model.AccountPassword == adminPassword)
            {
                HttpContext.Session.SetString("UserId", "0");
                HttpContext.Session.SetString("UserName", "Admin");
                HttpContext.Session.SetString("Role", "Admin");
                return RedirectToAction("Index", "Home");
            }
            var user = _contextAccount.GetAccountByEmail(model.AccountEmail);
            HttpContext.Session.SetString("UserId", user.AccountId.ToString());
            HttpContext.Session.SetString("UserName", user.AccountName);
            HttpContext.Session.SetString("Role", user.AccountRole == 2 ? "Lecturer" : "Staff");


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.AccountName),
                new Claim(ClaimTypes.Email, user.AccountEmail),
                new Claim(ClaimTypes.Role, user.AccountRole == 2 ? "Lecturer" : "Staff")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }

        // GET: SystemAccounts
        public async Task<IActionResult> Index()
        {
            var myStoreContext = _contextAccount.GetAccounts();
            return View(myStoreContext.ToList());
        }

        // GET: SystemAccounts/Details/5
        public async Task<IActionResult> Details(short id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var account = _contextAccount.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: SystemAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccount systemAccount)
        {
            if (ModelState.IsValid)
            {
                _contextAccount.SaveAccount(systemAccount);
                return RedirectToAction(nameof(Index));
            }
            return View(systemAccount);
        }

        // GET: SystemAccounts/Edit/5
        public async Task<IActionResult> Edit(short id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _contextAccount.GetAccountById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: SystemAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("AccountId,AccountName,AccountEmail,AccountRole")] SystemAccount systemAccount)
        {
            if (id != systemAccount.AccountId)
            {
                return NotFound();
            }

            var loggedInUser = GetCurrentUser(); // Lấy thông tin user đang đăng nhập
            if (loggedInUser == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contextAccount.UpdateAccount(systemAccount, loggedInUser);
                }
                catch (Exception)
                {
                    if (_contextAccount.GetAccountById(systemAccount.AccountId) == null)
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
            return View(systemAccount);

        }

        private SystemAccount GetCurrentUser()
        {
            var loggedInUserId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return null; // Không có user đăng nhập
            }

            // 🔹 Lấy đầy đủ thông tin user từ database
            return _contextAccount.GetAccountById(short.Parse(loggedInUserId));
        }

        // GET: SystemAccounts/Delete/5
        public async Task<IActionResult> Delete(short id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _contextAccount.GetAccountById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: SystemAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var account = _contextAccount.GetAccountById(id);
            if (account != null)
            {
                _contextAccount.DeleteAccount(account);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SystemAccountExists(short id)
        {
            var tmp = _contextAccount.GetAccountById(id);
            return (tmp != null) ? true : false;
        }

        
    }
}
