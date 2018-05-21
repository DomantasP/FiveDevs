namespace FiveDevsShop.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string Sku_code { get; set; }

        public int Category_id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int Discount { get; set; }
    }
}
