using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveDevsShop.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Title { get; set; }

        public int Tier { get; set; }
    }
}
