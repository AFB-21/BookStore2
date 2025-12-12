namespace BookStore.Application.DTOs.Book
{
    public class CreateBookDTO
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public decimal Price { get; set; }
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
