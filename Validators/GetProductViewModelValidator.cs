using FiveDevsShop.Models;
using FluentValidation;

namespace FiveDevsShop.Validators
{
    public class GetProductViewModelValidator : AbstractValidator<GetProductViewModel>
    {
        public GetProductViewModelValidator()
        {
            RuleFor(product => product.ProductCount)
                .Must(count => count > 0)
                .WithMessage("Dedami į krepšelį privalote pasirinkti bent vieną produktą");
        }
    }
}
