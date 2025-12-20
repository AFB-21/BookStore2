namespace BookStore.Application.DTOs.Book
{
    public class BookCardDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string AuthorName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
