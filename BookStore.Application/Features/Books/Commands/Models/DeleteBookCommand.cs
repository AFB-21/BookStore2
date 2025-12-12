using BookStore.Application.DTOs.Book;
using MediatR;

namespace BookStore.Application.Features.Books.Commands.Models
{
    public record DeleteBookCommand(Guid Id) : IRequest<BookDTO>;

}
