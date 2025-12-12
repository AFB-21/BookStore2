using BookStore.Application.Features.Authors.Commands.Models;
using FluentValidation;

namespace BookStore.Application.Features.Authors.Commands.Validators
{
    public class DeleteAuthorCommandValidators : AbstractValidator<DeleteAuthorCommand>
    {
        public DeleteAuthorCommandValidators()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Author Id is required.");
        }
    }

}
