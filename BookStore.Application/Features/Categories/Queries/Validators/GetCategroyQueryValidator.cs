using BookStore.Application.Features.Categories.Queries.Models;
using FluentValidation;

namespace BookStore.Application.Features.Categories.Queries.Validators
{
    public class GetCategroyQueryValidator : AbstractValidator<GetCategoryQuery>
    {
        public GetCategroyQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Category Id must not be empty.")
                .NotEqual(Guid.Empty).WithMessage("Category Id must not be an empty GUID.");
        }
    }
}
