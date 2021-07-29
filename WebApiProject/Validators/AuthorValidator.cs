using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApiProject.Data;
using WebApiProject.DTOs;

namespace WebApiProject.Validators
{
    public class AuthorValidator : AbstractValidator<AuthorDTO>
    {
        public AuthorValidator(DataContext dataContext)
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        }
    }
}
