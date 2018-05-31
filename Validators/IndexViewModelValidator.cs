using FiveDevsShop.Models.ManageViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Validators
{
    public class IndexViewModelValidator: AbstractValidator<IndexViewModel>
    {
        public IndexViewModelValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("El. Pašto laukelis negali būti paliktas tuščias.")
                .EmailAddress()
                .WithMessage("Įvesti duomenys nėra el. pašto adresas.");

            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("Vardo laukelis negali būti paliktas tuščias.")
                .MinimumLength(2)
                .WithMessage("Vardas negali būti trumpesnis nei 2 simboliai.")
                .MaximumLength(45)
                .WithMessage("Vardas negali būti ilgesnis nei 45 simboliai.")
                .Matches(@"^[A-Za-zĄąČčĘęĖėĮįŠšŲųŪūŽž]+$")
                .WithMessage("Vardą gali sudaryti tik raidės.");

            RuleFor(user => user.LastName)
                .NotEmpty()
                .WithMessage("Pavardės laukelis negali būti paliktas tuščias.")
                .MinimumLength(2)
                .WithMessage("Pavardė negali būti trumpesnė nei 2 simboliai.")
                .MaximumLength(45)
                .WithMessage("Pavardė negali būti ilgesnė nei 45 simboliai.")
                .Matches(@"^[A-Za-zĄąČčĘęĖėĮįŠšŲųŪūŽž]+$")
                .WithMessage("Pavardę gali sudaryti tik raidės.");

            RuleFor(user => user.PhoneNumber)
                .Matches(@"^[0-9]*$")
                .WithMessage("Telefono numerį gali sudaryti tik skaitmenys");

            RuleFor(user => user.City)
                .NotEmpty()
                .WithMessage("Miesto laukelis negali būti paliktas tuščias.")
                .MaximumLength(45)
                .WithMessage("Miesto pavadinimas negali būti ilgesnis nei 45 simboliai.");

            RuleFor(user => user.Street)
                .NotEmpty()
                .WithMessage("Gatvės laukelis negali būti paliktas tuščias.")
                .MaximumLength(45)
                .WithMessage("Gatvės pavadinimas negali būti ilgesnis nei 45 simboliai.");

            RuleFor(user => user.HouseNumber)
                .NotEmpty()
                .WithMessage("Namo numerio laukelis negali būti paliktas tuščias.")
                .MaximumLength(10)
                .WithMessage("Namo numerio negali sudaryti daugiau nei 10 simbolių.");

            RuleFor(user => user.ApartmentNumber)
                .MaximumLength(10)
                .WithMessage("Buto numerio negali sudaryti daugiau nei 10 simbolių.");

            RuleFor(user => user.PostalCode)
                .NotEmpty()
                .WithMessage("Pašto kodo laukelis negali būti paliktas tuščias.")
                .Matches(@"^[0-9]{5}$")
                .WithMessage("Pašto kodą turi sudaryti 5 skaičiai.");
        }
    }
}
