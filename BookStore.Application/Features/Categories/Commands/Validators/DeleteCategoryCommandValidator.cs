using BookStore.Application.Features.Categories.Commands.Models;
using FluentValidation;

namespace BookStore.Application.Features.Categories.Commands.Validators
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
