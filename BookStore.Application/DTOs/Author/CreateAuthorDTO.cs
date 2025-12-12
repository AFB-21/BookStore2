namespace BookStore.Application.DTOs.Author
{
    public class CreateAuthorDTO
    {
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }
    }
}
