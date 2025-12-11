using BookStore.Application.DTOs.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Categories.Commands.Models
{
    public record UpdateCategoryCommand(Guid Id, UpdateCategoryDTO DTO):IRequest<CategoryDTO>;
    
}
