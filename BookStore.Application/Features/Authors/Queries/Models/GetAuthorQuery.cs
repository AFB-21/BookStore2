using BookStore.Application.DTOs.Author;
using MediatR;

namespace BookStore.Application.Features.Authors.Queries.Models
{
    public record GetAuthorQuery(Guid Id) : IRequest<AuthorDTO?>;
}
