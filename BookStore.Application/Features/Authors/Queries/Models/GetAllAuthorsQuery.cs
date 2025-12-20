using BookStore.Application.DTOs.Author;
using MediatR;

namespace BookStore.Application.Features.Authors.Queries.Models
{
    public record GetAllAuthorsQuery() : IRequest<List<AuthorSummaryDTO>>;

}
