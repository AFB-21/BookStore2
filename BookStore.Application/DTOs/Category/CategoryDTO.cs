using BookStore.Application.DTOs.Book;

namespace BookStore.Application.DTOs.Category
{
    public record CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        // Use BookSummaryDTO instead of BookDTO
        public List<BookSummaryDTO> Books { get; set; } = new();

    }

}
