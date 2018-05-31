using FiveDevsShop.Models.AccountViewModels;
using FluentValidation; 

namespace FiveDevsShop.Validators
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(user => user.UserName)
                .NotEmpty()
                .WithMessage("Vartotojo vardo laukelis negali būti paliktas tuščias.")
                .MaximumLength(45)
                .WithMessage("Vartotojo vardas negali būti ilgesnis nei 45 simboliai.")
                .Matches(@"^[A-Za-zĄąČčĘęĖėĮįŠšŲųŪūŽž\-._@]+$")
                .WithMessage("Vartotojo vardą gali sudaryti tik raidės, skaičiai bei -._@ simboliai.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("El. Pašto laukelis negali būti paliktas tuščias.")
                .EmailAddress()
                .WithMessage("Įvesti duomenys nėra el. pašto adresas.");

            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Slaptažodžio laukelis negali būti paliktas tuščias.")
                .Matches("^(?=.*[a-ząčęėįšųūž])(?=.*[A-ZĄČĘĖĮŠŲŪŽ])(?=.*[^a-zA-ZĄąČčĘęĖėĮįŠšŲųŪūŽž])(.{8,})$")
                .WithMessage("Slaptažodį turi sudaryti bent 8 simboliai, didžioji raidė, mažoji raidė bei ne raidinis simbolis.");

            RuleFor(user => user.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Slaptažodžio pakartojimo laukelis negali būti paliktas tuščias.");

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

