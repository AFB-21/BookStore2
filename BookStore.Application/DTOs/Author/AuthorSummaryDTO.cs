namespace BookStore.Application.DTOs.Author
{
    public class AuthorSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int BookCount { get; set; }  // Just the count, not the books themselves
    }
}
