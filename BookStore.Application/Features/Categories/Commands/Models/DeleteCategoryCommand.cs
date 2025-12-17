using BookStore.Application.DTOs.Category;
using MediatR;

namespace BookStore.Application.Features.Categories.Commands.Models
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<Result<CategoryDTO>>;

}
