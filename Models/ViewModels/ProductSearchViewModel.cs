using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class ProductSearchViewModel
    {
        public string Query { get; set; }

        public ProductListViewModel Products { get; set; }
    }
}
