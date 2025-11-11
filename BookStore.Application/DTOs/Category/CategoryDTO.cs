using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.DTOs.Category
{
    public record CategoryDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

    }

}
