using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class CategoryViewModel
    {
        /// <summary>
        /// Currently viewed category. Null if we are at the root.
        /// </summary>
        public Category Current { get; set; }

        public IEnumerable<Category> Subcategories { get; set; }

        public ProductListViewModel Products { get; set; }

        public bool IsAtRoot => Current == null;
    }
}
