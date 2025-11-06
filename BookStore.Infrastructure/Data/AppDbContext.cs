using BookStore.Domain.Entities;
using BookStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Data
{
     public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {}

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Category> Categories { get; set; }=null!;
        public DbSet<Author> Authors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Book>().Property(b=> b.Title).IsRequired().HasMaxLength(200);
            builder.Entity<Author>().Property(b=> b.Name).IsRequired().HasMaxLength(150);
            builder.Entity<Category>().Property(b=> b.Name).IsRequired().HasMaxLength(200);
        }
    }
}
