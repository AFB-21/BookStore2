using BookStore.Application.DTOs.Book;
using MediatR;

namespace BookStore.Application.Features.Books.Queries.Models
{
    public record GetDeletedBooksQuery(Guid Id) : IRequest<List<BookSummaryDTO>>;

}
