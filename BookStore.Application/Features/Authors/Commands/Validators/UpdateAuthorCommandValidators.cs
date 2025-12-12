using BookStore.Application.Features.Authors.Commands.Models;
using FluentValidation;

namespace BookStore.Application.Features.Authors.Commands.Validators
{
    public class UpdateAuthorCommandValidators : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidators()
        {
            RuleFor(x => x.DTO.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DTO.Bio).MaximumLength(1000);
        }
    }
}
