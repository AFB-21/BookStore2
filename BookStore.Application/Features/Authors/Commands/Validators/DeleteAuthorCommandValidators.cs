using BookStore.Application.Features.Authors.Commands.Models;
using BookStore.Application.Features.Books.Commands.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
