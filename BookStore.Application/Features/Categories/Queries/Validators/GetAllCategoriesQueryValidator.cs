using BookStore.Application.Features.Categories.Queries.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Categories.Queries.Validators
{
    public class GetAllCategoriesQueryValidator: AbstractValidator<GetAllCategoriesQuery>
    {
    }
}
