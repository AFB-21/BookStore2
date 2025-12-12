using BookStore.Application.DTOs.Author;
using MediatR;

namespace BookStore.Application.Features.Authors.Commands.Models
{
    public record UpdateAuthorCommand(Guid Id, UpdateAuthorDTO DTO) : IRequest<AuthorDTO>;
}
