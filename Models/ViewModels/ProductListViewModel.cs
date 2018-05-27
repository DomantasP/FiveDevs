using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductPreviewModel> Products { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }
    }
}
