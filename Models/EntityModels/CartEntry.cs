using System;

namespace FiveDevsShop.Models
{
    [Serializable]
    public class CartEntry
    {
        public int Id { get; set; }
        public string SkuCode { get; set; }
        public string Title { get; set; }
        public decimal NormalPrice { get; set; }
        public int Discount { get; set; }
        public string ImageUrl { get; set; }
        public int Amount { get; set; }
        public decimal FinalPrice { get; set; }
        public string Category { get; set; }
    }

    
}
