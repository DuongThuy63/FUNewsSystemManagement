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
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly FUNewsManagementSystemContext _context;
        private readonly IAccountService _contextAccount;

        public AccountController(FUNewsManagementSystemContext context, IAccountService contextAccount )
        {
            _context = context;
            _contextAccount = contextAccount;
        }

        

        // GET: Account/Details/5
        public async Task<IActionResult> Details()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return NotFound();
            }

            short userId = short.Parse(userIdClaim);

            var systemAccount = _contextAccount.GetAccountById(userId);
                

            if (systemAccount == null)
            {
                return NotFound();
            }

            return View(systemAccount);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return NotFound();
            }

            short userId = short.Parse(userIdClaim);

            var systemAccount = _contextAccount.GetAccountById(userId);
            if (systemAccount == null)
            {
                return NotFound();
            }
            return View(systemAccount);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("AccountId,AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccount systemAccount)
        {
            if (id != systemAccount.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemAccountExists(systemAccount.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = systemAccount.AccountId });
            }
            return View(systemAccount);
        }

        
        private bool SystemAccountExists(short id)
        {
            return _context.SystemAccount.Any(e => e.AccountId == id);
        }
    }
}
