using BookStore.Application.DTOs.Author;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Authors.Commands.Models
{
    public record CreateAuthorCommand(CreateAuthorDTO DTO) : IRequest<AuthorDTO>;
    
}
