using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs.Author
{
    public record AuthorDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string? Bio { get; init; }
    }

}
