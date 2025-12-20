using BookStore.Application.Common;
using BookStore.Application.DTOs.Book;
using MediatR;

namespace BookStore.Application.Features.Books.Commands.Models
{
    public record HardDeleteBookCommand(Guid Id) : IRequest<Result<BookDTO>>;
}
