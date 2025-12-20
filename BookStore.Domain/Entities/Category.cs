using BookStore.Domain.Common;

namespace BookStore.Domain.Entities
{
    public class Category: BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
