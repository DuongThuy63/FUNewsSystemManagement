using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using FUNewsManagementSystem.Data;
using Services;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem.Controllers
{
    
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewBag.Message = "Chào mừng Admin!";
            ViewBag.Pages = new List<object>
        {
            new { Name = "Quản lý tài khoản", Url = Url.Action("Index", "SystemAccounts") },
            new { Name = "Quản lý danh mục", Url = Url.Action("Index", "Categories") }
        };

            return View();
        }
    }
}
