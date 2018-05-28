using FiveDevsShop.Models.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class CategoryViewModel
    {
        public List<Category> CategoryPath { get; set; } = new List<Category>();

        public List<CategoryTree.Node> Subtrees { get; set; }

        public ProductListViewModel Products { get; set; }

        public bool IsAtRoot => CategoryPath.Count == 0;

        public Category Current => IsAtRoot ? null : CategoryPath.Last();
    }
}
