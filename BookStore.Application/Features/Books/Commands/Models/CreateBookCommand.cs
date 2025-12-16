using BookStore.Application.Common;
using BookStore.Application.DTOs.Book;
using MediatR;

namespace BookStore.Application.Features.Books.Commands.Models
{
    public record CreateBookCommand(CreateBookDTO DTO) : IRequest<Result<BookDTO>>;
}
