using BookStore.Application.DTOs.Book;

namespace BookStore.Application.DTOs.Author
{
    public record AuthorDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }

        // Use BookSummaryDTO instead of BookDTO to break circular reference
        public List<BookSummaryDTO> Books { get; set; } = new();
    }
}
