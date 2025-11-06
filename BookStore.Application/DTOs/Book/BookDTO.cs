using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs.Book
{
    public record BookDTO(Guid Id, string Title, string? Description, DateTime PublishedOn, decimal Price, Guid AuthorId, Guid CategoryId);

}
