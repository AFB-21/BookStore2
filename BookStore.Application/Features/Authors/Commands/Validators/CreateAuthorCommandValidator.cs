using BookStore.Application.Features.Authors.Commands.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Authors.Commands.Validators
{
    public class CreateAuthorCommandValidator:AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(x => x.DTO.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DTO.Bio).MaximumLength(1000);
        }
    }
}
