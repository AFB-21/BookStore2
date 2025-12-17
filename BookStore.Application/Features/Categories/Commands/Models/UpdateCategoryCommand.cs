using BookStore.Application.Common;
using BookStore.Application.DTOs.Category;
using MediatR;

namespace BookStore.Application.Features.Categories.Commands.Models
{
    public record UpdateCategoryCommand(Guid Id, UpdateCategoryDTO DTO) : IRequest<Result<CategoryDTO>>;

}
