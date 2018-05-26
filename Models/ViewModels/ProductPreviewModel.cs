using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class ProductPreviewModel
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public string MainImageUrl { get; set; }
    }
}
