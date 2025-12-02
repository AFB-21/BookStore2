using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs.Book
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public decimal Price { get; set; }

        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
