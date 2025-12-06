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
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Book>().Property(b => b.Title).IsRequired().HasMaxLength(200);
            // ensure PublishedOn maps to the existing column name in the DB (misspelled in migrations)
            builder.Entity<Book>().Property(b => b.PublishedOn).HasColumnName("PuplishedOn").HasColumnType("datetime2");
            builder.Entity<Author>().Property(b => b.Name).IsRequired().HasMaxLength(150);
            builder.Entity<Category>().Property(b => b.Name).IsRequired().HasMaxLength(200);

            //seed data
            builder.Entity<Category>().HasData(
                new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Science Fiction" },
                new Category { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Fantasy" },
                new Category { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Mystery" }
            );
            builder.Entity<Author>().HasData(
                new Author { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Isaac Asimov", Bio = "Prolific science fiction author." },
                new Author { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "J.K. Rowling", Bio = "Author of the Harry Potter series." },
                new Author { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Agatha Christie", Bio = "Famous mystery novelist." }
            );
            builder.Entity<Book>().HasData(
                new Book
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Title = "Foundation",
                    Description = "A science fiction novel about the fall of the Galactic Empire.",
                    PublishedOn = new DateTime(1951, 6, 1),
                    Price = 9.99m,
                    AuthorId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },
                new Book
                {
                    Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Description = "The first book in the Harry Potter series.",
                    PublishedOn = new DateTime(1997, 6, 26),
                    Price = 12.99m,
                    AuthorId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                },
                new Book
                {
                    Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                    Title = "Murder on the Orient Express",
                    Description = "Murder on the Orient Express",
                    PublishedOn = new DateTime(1971, 10, 1),
                    Price = 15.19m,
                    AuthorId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    CategoryId = Guid.Parse("33333333-3333-3333-3333-333333333333")
                }
            );

        }
    }
}