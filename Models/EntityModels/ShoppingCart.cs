using FiveDevsShop.Models.DomainServices;
using FiveDevsShop.Services;
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

        public void AddProduct(Product product, int amount, string categoryName, IImageUploader uploader, PriceCalculator calculator)
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
                    ImageUrl = uploader.GetImageUrl(product.MainImageId),
                    Amount = amount,
                    FinalPrice = calculator.CalculateFinalPrice(product, amount),
                    Category = categoryName,
                };
            }
        }
        public void increaseAmountByOne(String skuCode)
        {
            if (Entries.ContainsKey(skuCode))
            {
                Entries[skuCode].Amount++;
            }

        }
        public void decreaseAmountByOne(String skuCode)
        {
            if (Entries.ContainsKey(skuCode))
            {
                if (Entries[skuCode].Amount > 1) Entries[skuCode].Amount--;
                else removeFromCart(skuCode);
            }
        }
        public void removeFromCart(String skuCode)
        {
            if (Entries.ContainsKey(skuCode)) Entries.Remove(skuCode);
        }
        public decimal TotalCost()
        {
            return Entries.Values.Sum(entry => entry.FinalPrice);
        }
    }
}
