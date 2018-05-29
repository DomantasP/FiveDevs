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

        public void AddProduct(Product product, int amount)
        {
            if (Entries.ContainsKey(product.SkuCode))
            {
                // TODO: check if this is actually the same product
                Entries[product.SkuCode].Amount += amount;
            }
            else
            {
                Entries[product.SkuCode] = new CartEntry()
                {
                    Product = product,
                    Amount = amount,
                };
            }
        }

        public decimal TotalCost()
        {
            decimal total = 0;
            foreach (var entry in Entries.Values)
            {
                // TODO: discounts
                total += entry.Product.Price * entry.Amount;
            }
            return total;
        }
    }
}
