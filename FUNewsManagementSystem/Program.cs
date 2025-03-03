using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FUNewsManagementSystem.Data;
using Services;
using System.Globalization;
using Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using BusinessObjects;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FUNewsManagementSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyStockDB")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/SystemAccounts/Login"; // Trang đăng nhập
        options.AccessDeniedPath = "/Home/AccessDenied"; // Trang từ chối truy cập
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hết hạn cookie
    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax; 
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

builder.Services.AddAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
