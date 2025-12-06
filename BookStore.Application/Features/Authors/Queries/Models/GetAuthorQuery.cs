using BookStore.Application.DTOs.Author;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Authors.Queries.Models
{
    public record GetAuthorQuery(Guid Id): IRequest<AuthorDTO?>;
}
