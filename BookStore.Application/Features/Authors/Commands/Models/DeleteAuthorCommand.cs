using BookStore.Application.DTOs.Author;
using MediatR;

namespace BookStore.Application.Features.Authors.Commands.Models
{
    public record DeleteAuthorCommand(Guid Id) : IRequest<AuthorDTO>;
}
