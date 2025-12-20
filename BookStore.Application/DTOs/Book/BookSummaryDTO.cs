namespace BookStore.Application.DTOs.Book
{
    public class BookSummaryDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime PublishedOn { get; set; }

        // Only IDs and names - no nested objects
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
