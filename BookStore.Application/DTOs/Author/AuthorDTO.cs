using BookStore.Application.DTOs.Book;

namespace BookStore.Application.DTOs.Author
{
    public record AuthorDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string? Bio { get; init; }

        // Include author's books in the DTO
        public List<BookDTO> Books { get; init; } = new List<BookDTO>();
    }
}
