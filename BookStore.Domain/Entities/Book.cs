using BookStore.Domain.Common;

namespace BookStore.Domain.Entities
{
    public class Book : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public decimal Price { get; set; }

        // FK relationships
        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
