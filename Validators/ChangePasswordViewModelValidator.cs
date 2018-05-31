using FiveDevsShop.Models.ManageViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Validators
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(user => user.OldPassword)
                .NotEmpty()
                .WithMessage("Dabartinio slaptažodžio laukelis negali būti paliktas tuščias.");

            RuleFor(user => user.NewPassword)
                .NotEmpty()
                .WithMessage("Naujo slaptažodžio laukelis negali būti paliktas tuščias.")
                .Matches("^(?=.*[a-ząčęėįšųūž])(?=.*[A-ZĄČĘĖĮŠŲŪŽ])(?=.*[^a-zA-ZĄąČčĘęĖėĮįŠšŲųŪūŽž])(.{8,})$")
                .WithMessage("Naują slaptažodį turi sudaryti bent 8 simboliai, didžioji raidė, mažoji raidė bei ne raidinis simbolis.");

            RuleFor(user => user.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Naujo slaptažodžio pakartojimo laukelis negali būti paliktas tuščias.");
        }
    }
}
