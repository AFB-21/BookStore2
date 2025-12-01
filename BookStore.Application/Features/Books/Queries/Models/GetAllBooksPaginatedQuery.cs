using BookStore.Application.DTOs.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Books.Queries.Models
{
    public record GetAllBooksPaginatedQuery(
        int PageNumber,
        int PageSize
        //string? filter,
        //string? sortBy,
        //bool desc
        ) : IRequest<List<BookDTO?>>;
    
    
}
