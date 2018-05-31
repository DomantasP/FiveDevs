using FiveDevsShop.Models;
using FluentValidation;
using System.Linq;

namespace FiveDevsShop.Validators
{
    public class PaymentViewModelValidator : AbstractValidator<PaymentViewModel>
    {
        public PaymentViewModelValidator()
        {
            RuleFor(model => model.CCV)
                .Must(c => IsCcvValid(c))
                .WithMessage("CCV turi būti iš trijų skaitmenų");

            RuleFor(model => model.ExpYear)
                .Must(y => y >= 1970)
                .WithMessage("Kortelės galiojimo metai per seni");

            RuleFor(model => model.Name)
                .Must(n => n != null && n.Length >= 2 && n.Length <= 32)
                .WithMessage("Vardas turi būti nuo 2 iki 32 simbolių ilgio");

            RuleFor(model => model.Number)
                .Must(n => IsNumberValid(n))
                .WithMessage("Kortelės numeris turi būti iš 16 skaitmenų");

            RuleFor(model => model.Number)
                .Must(n => IsNumberValid(n) && LuhnCheck(n))
                .WithMessage("Blogas kortelės numeris");
        }

        private bool IsNumberValid(string val)
        {
            if (val == null) return false;
            if (val.Length != 16) return false;
            if (val.Any(c => c < '0' || c > '9')) return false;
            return true;
        }

        private bool IsCcvValid(string val)
        {
            if (val == null) return false;
            if (val.Length != 3) return false;
            if (val.Any(c => c < '0' || c > '9')) return false;
            return true;
        }

        private bool LuhnCheck(string num)
        {
            int total = 0;
            for (int i = 0; i < 16; i += 2)
            {
                int x = (num[i] - '0') * 2;
                if (x >= 10) x = x / 10 + x % 10;
                total += x;
                total += num[i + 1] - '0';
            }
            return total % 10 == 0;
        }
    }
}
