using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Data;
using WebApiProject.DTOs;

namespace WebApiProject.Validators
{
    public class UserValidator :  AbstractValidator<RegisterReqDTO>
    {
        public UserValidator(DataContext dataContext)
        {
            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var userAlreadyExists = dataContext.Users.Any(c => c.Email == value);
                if (userAlreadyExists)
                {
                    context.AddFailure("Email", "That email is already taken");
                }
            });
            RuleFor(x => x.Mobile).NotEmpty().MaximumLength(10);
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(8);
        }
    }
}
