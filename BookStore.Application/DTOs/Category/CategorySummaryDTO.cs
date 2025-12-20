namespace BookStore.Application.DTOs.Category
{
    public class CategorySummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int BookCount { get; set; }
    }
}
