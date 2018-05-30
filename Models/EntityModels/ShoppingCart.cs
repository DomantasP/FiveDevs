using FiveDevsShop.Models.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    [Serializable]
    public class ShoppingCart
    {
        public Dictionary<string, CartEntry> Entries { get; set; } = new Dictionary<string, CartEntry>();

        public void AddProduct(Product product, int amount, string categoryName, PriceCalculator calculator)
        {
            if (Entries.ContainsKey(product.SkuCode))
            {
                // TODO: check if this is actually the same product
                var entry = Entries[product.SkuCode];
                entry.Amount += amount;
                entry.FinalPrice += calculator.CalculateFinalPrice(product, amount);
            }
            else
            {
                Entries[product.SkuCode] = new CartEntry()
                {
                    Id = product.Id,
                    SkuCode = product.SkuCode,
                    Title = product.Title,
                    NormalPrice = product.Price,
                    Discount = product.Discount,
                    ImageUrl = FiveDevsShop.Services.CloudinaryClient.GetImageUrl(product.MainImageId),
                    Amount = amount,
                    FinalPrice = calculator.CalculateFinalPrice(product, amount),
                    Category = categoryName,
                };
            }
        }

        public decimal TotalCost()
        {
            return Entries.Values.Sum(entry => entry.FinalPrice);
        }
    }
}
