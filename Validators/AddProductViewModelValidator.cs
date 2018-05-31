using FiveDevsShop.Models;
using FluentValidation;

namespace FiveDevsShop.Validators
{
    public class AddProductViewModelValidator : AbstractValidator<AddProductViewModel>
    {
        public AddProductViewModelValidator()
        {
            RuleFor(product => product.Title)
                .NotEmpty()
                .WithMessage("Pavadinimas privalomas");
            
            RuleFor(product => product.Description)
                .NotEmpty()
                .WithMessage("Aprašymas privalomas");
            
            RuleFor(product => product.ShortDescription)
                .NotEmpty()
                .WithMessage("Trumpasis aprašymas privalomas");

            RuleFor(product => product.Price)
                .NotEmpty()
                .WithMessage("Kaina privaloma")
                .Must(price => price > 0)
                .WithMessage("Kaina privalo būti didesnė už nulį");
            
            RuleFor(product => product.Discount)
                .Must(discount => discount == 0 || discount < 99)
                .WithMessage("Nuolaida gali būti tarp nulio ir devyniasdešimt devynių procentų");

            RuleFor(product => product.SkuCode)
                .NotEmpty()
                .WithMessage("Sku kodas privalomas");
            
            RuleFor(product => product.CategoryId)
                .NotNull()
                .WithMessage("Kategorija privaloma");


        }
    }
}
