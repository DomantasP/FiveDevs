namespace FiveDevsShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string SkuCode { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string MainImageId { get; set; }

        public int Discount { get; set; }
    }
}
