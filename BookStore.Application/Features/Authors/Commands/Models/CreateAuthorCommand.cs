using BookStore.Application.DTOs.Author;
using MediatR;

namespace BookStore.Application.Features.Authors.Commands.Models
{
    public record CreateAuthorCommand(CreateAuthorDTO DTO) : IRequest<AuthorDTO>;

}
