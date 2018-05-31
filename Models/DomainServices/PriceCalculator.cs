using System;

namespace FiveDevsShop.Models.DomainServices
{
    public class PriceCalculator
    {
        public decimal CalculateDiscountedPrice(Product product)
        {
            decimal multiplier = (100 - product.Discount) / 100m;
            return Math.Round(product.Price * multiplier, 2);
        }

        public decimal CalculateFinalPrice(Product product, int amount)
        {
            decimal single = CalculateDiscountedPrice(product);
            return single * amount;
        }
    }
}
