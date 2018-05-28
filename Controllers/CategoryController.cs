using FiveDevsShop.Data;
using FiveDevsShop.Models;
using FiveDevsShop.Models.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;

        public CategoryController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult GetCategoryAndSubcategories(int? id, int page = 1)
        {
            Category current;

            var tree = new CategoryTree(db.Category);

            if (id == null)
                current = null;
            else
                current = tree.FindCategoryNode(id.Value).Category;

            var subtrees = tree.Subtrees(id).ToList();
            var products = ShowProductsInCategory(tree, id, page);

            return View(new CategoryViewModel()
            {
                Current = current,
                Subtrees = subtrees,
                Products = products,
            });
        }

        private ProductListViewModel ShowProductsInCategory(CategoryTree tree, int? id, int page)
        {
            return Paging.LoadPage(FindProductsInCategory(tree, id), page);
        }

        // TODO: this loads all products eagerly, even though we throw away most of them
        private List<Product> FindProductsInCategory(CategoryTree tree, int? id)
        {
            List<Product> products = null;

            if (id == null)
            {
                // we are at root, return all products
                products = db.Product.ToList();
            }
            else
            {
                products = new List<Product>();
                var categories = db.Category.ToList();
                var subtree = tree.FindCategoryNode(id.Value).CategoriesInSubtree();
                foreach (var category in subtree)
                {
                    var newProducts = db.Product.Where(p => p.CategoryId == category.Id);
                    products.AddRange(newProducts);
                }
            }

            return products;
        }
    }
}
