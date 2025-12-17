using BookStore.Application.DTOs.Category;
using MediatR;

namespace BookStore.Application.Features.Categories.Commands.Models
{
    public record CreateCategoryCommand(CreateCategoryDTO DTO) : IRequest<Result<CategoryDTO>>;


}
