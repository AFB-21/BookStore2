using BookStore.Application.Features.Authors.Commands.Models;
using FluentValidation;

namespace BookStore.Application.Features.Authors.Commands.Validators
{
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(x => x.DTO.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DTO.Bio).MaximumLength(1000);
        }
    }
}
