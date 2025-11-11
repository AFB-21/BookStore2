using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs.Book
{
    public record BookDTO
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string? Description { get; init; }
        public DateTime PublishedOn { get; init; }
        public decimal Price { get; init; }
        public Guid AuthorId { get; init; }
        public Guid CategoryId { get; init; }
    }

}
