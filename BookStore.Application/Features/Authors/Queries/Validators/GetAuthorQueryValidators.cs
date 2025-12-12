using BookStore.Application.Features.Authors.Queries.Models;
using FluentValidation;

namespace BookStore.Application.Features.Authors.Queries.Validators
{
    public class GetAuthorQueryValidators : AbstractValidator<GetAuthorQuery>
    {
        public GetAuthorQueryValidators()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Author Id must not be empty.");
        }
    }
}
