using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs.Author
{
    public record AuthorDTO(Guid Id, string Name, string? Bio);

}
