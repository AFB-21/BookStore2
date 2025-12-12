namespace BookStore.Application.DTOs.Category
{
    public record CategoryDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

    }

}
