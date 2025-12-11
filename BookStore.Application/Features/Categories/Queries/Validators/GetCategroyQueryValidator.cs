using BookStore.Application.Features.Categories.Queries.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Categories.Queries.Validators
{
    public class GetCategroyQueryValidator: AbstractValidator<GetCategoryQuery>
    {
        public GetCategroyQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Category Id must not be empty.")
                .NotEqual(Guid.Empty).WithMessage("Category Id must not be an empty GUID.");
        }
    }
}
