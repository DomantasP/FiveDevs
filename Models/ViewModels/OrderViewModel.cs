using System.Collections.Generic;

namespace FiveDevsShop.Models
{
    public class OrderViewModel
    {
            public int Id { get; set; }

            public string Date { get; set; }

            public int Status { get; set; }

            public List<Purchase> Items { get; set; }
    }
}