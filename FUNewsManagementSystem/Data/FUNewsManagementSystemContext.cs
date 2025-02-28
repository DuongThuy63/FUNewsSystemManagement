using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace FUNewsManagementSystem.Data
{
    public class FUNewsManagementSystemContext : DbContext
    {
        public FUNewsManagementSystemContext (DbContextOptions<FUNewsManagementSystemContext> options)
            : base(options)
        {
        }

        public DbSet<BusinessObjects.SystemAccount> SystemAccount { get; set; } = default!;
        public DbSet<BusinessObjects.NewsArticle> NewsArticle { get; set; } = default!;
        public DbSet<BusinessObjects.Category> Category { get; set; } = default!;
    }
}
