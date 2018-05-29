using FiveDevsShop.Models.AccountViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Validators
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("El. Pašto laukelis negali būti paliktas tuščias.")
                .EmailAddress()
                .WithMessage("Įvesti duomenys nėra el. pašto adresas.");
        }
    }
}
