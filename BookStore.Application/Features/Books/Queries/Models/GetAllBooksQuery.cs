using BookStore.Application.DTOs.Book;
using MediatR;

namespace BookStore.Application.Features.Books.Queries.Models
{
    public record GetAllBooksQuery() : IRequest<List<BookSummaryDTO>>;

}
