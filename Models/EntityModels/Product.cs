namespace FiveDevsShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        // Should product have only one category ?
        public int CategoryId { get; set; }

        public int Quantity { get; set; }
    }
}
