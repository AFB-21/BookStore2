using BookStore.Application.Features.Categories.Commands.Models;
using FluentValidation;

namespace BookStore.Application.Features.Categories.Commands.Validators
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.DTO.Name).NotEmpty().MaximumLength(100);
        }
    }
}
