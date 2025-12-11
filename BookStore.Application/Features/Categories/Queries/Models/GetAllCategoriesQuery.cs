using BookStore.Application.DTOs.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Categories.Queries.Models
{
    public record GetAllCategoriesQuery() : IRequest<List<CategoryDTO?>>;
    
}
