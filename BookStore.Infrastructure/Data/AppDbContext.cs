using BookStore.Application.Services;
using BookStore.Domain.Common;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly IUserContextService? _userContextService;
        public AppDbContext(DbContextOptions<AppDbContext> options,
                IUserContextService? userContextService = null) : base(options)
        {
            _userContextService = userContextService;
        }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Book>(entity =>
            {
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.RowVersion).IsRowVersion();

                // Global query filter - automatically exclude soft deleted items
                entity.HasQueryFilter(b => !b.IsDeleted);
            });
            builder.Entity<Author>(entity =>
            {
                entity.Property(a => a.Name).IsRequired().HasMaxLength(150);
                entity.Property(a => a.RowVersion).IsRowVersion();

                // Global query filter
                entity.HasQueryFilter(a => !a.IsDeleted);
            });
            // Configure Category entity
            builder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
                entity.Property(c => c.RowVersion).IsRowVersion();

                // Global query filter
                entity.HasQueryFilter(c => !c.IsDeleted);
            });

            //seed data
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Science Fiction",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Category
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Fantasy",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Category
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Mystery",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                }

            );
            builder.Entity<Author>().HasData(
                new Author
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Isaac Asimov",
                    Bio = "Prolific science fiction author.",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Author
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Name = "J.K. Rowling",
                    Bio = "Author of the Harry Potter series.",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Author
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Name = "Agatha Christie",
                    Bio = "Famous mystery novelist.",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                }
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
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Book
                {
                    Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Description = "The first book in the Harry Potter series.",
                    PublishedOn = new DateTime(1997, 6, 26),
                    Price = 12.99m,
                    AuthorId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Book
                {
                    Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                    Title = "Murder on the Orient Express",
                    Description = "Murder on the Orient Express",
                    PublishedOn = new DateTime(1971, 10, 1),
                    Price = 15.19m,
                    AuthorId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    CategoryId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            );

        }
        // Override SaveChanges to automatically set audit fields
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        private void SetAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                           (e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.CreatedBy = GetCurrentUser();
                        break;

                    case EntityState.Modified:
                        entity.UpdatedAt = DateTime.UtcNow;
                        entity.UpdatedBy = GetCurrentUser();
                        break;

                    case EntityState.Deleted:
                        // Convert hard delete to soft delete
                        entry.State = EntityState.Modified;
                        entity.IsDeleted = true;
                        entity.DeletedAt = DateTime.UtcNow;
                        entity.DeletedBy = GetCurrentUser();
                        break;
                }
            }
        }

        private string? GetCurrentUser()
        {
            return _userContextService?.GetCurrentUserName() ?? "System";
        }
    }
}