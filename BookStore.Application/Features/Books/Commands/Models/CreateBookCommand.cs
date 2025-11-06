using BookStore.Application.DTOs.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Books.Commands.Models
{
    public record CreateBookCommand(CreateBookDTO DTO):IRequest<BookDTO>;
}
