using BookStore.Application.DTOs.Book;
using MediatR;

namespace BookStore.Application.Features.Books.Commands.Models
{
    public record UpdateBookCommand(Guid Id, UpdateBookDTO DTO) : IRequest<BookDTO>;

}
