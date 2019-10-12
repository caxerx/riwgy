using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using riwgy.Model;

namespace riwgy.Models
{
    public class RiwgyDbContext : DbContext
    {
        public RiwgyDbContext (DbContextOptions<RiwgyDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlMapping>().HasIndex(p => new { p.Riwgy }).IsUnique();
        }

        public DbSet<UrlMapping> UrlMapping { get; set; }
    }
}
