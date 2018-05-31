using FiveDevsShop.Models;
using FluentValidation;

namespace FiveDevsShop.Validators
{
    public class CategoryAddViewModelValidator : AbstractValidator<CategoryAddViewModel>
    {
        public CategoryAddViewModelValidator()
        {
            RuleFor(cat => cat.Title)
                .NotEmpty()
                .WithMessage("Pavadinimas privalomas");
        }
    }
}
