using BookStore.Application.DTOs.Category;
using MediatR;

namespace BookStore.Application.Features.Categories.Queries.Models
{
    public record GetCategoryQuery(Guid Id) : IRequest<CategoryDTO?>;

}
