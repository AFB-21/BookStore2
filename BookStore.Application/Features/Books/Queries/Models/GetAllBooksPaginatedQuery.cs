using BookStore.Application.DTOs.Book;
using MediatR;

namespace BookStore.Application.Features.Books.Queries.Models
{
    public record GetAllBooksPaginatedQuery(
        int PageNumber,
        int PageSize
        //string? filter,
        //string? sortBy,
        //bool desc
        ) : IRequest<List<BookDTO?>>;


}
