using FiveDevsShop.Data;
using FiveDevsShop.Models;
using FiveDevsShop.Models.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public IActionResult GetCategoryAndSubcategories(int? id)
        {
            Category current;
            IEnumerable<Category> subcategories;

            if(id == null)
            {
                current = null;
                subcategories = db.Category.Where(c => c.Parent_id == null).ToList();
            }
            else
            {
                current = db.Category.Find(id);
                subcategories = db.Category.Where(c => c.Parent_id == id);
            }

            return View(new CategoryViewModel()
            {
                Current = current,
                Subcategories = subcategories,
                Products = FindProductsInCategory(id),
            });
        }

        private IEnumerable<ProductPreviewModel> FindProductsInCategory(int? id)
        {
            // TODO: currently this loads all products fitting criteria

            if (id == null)
            {
                // we are at root, return all products
                return db.Product.Select(ProductPreviewModel.FromProduct);
            }

            var categories = db.Category;
            var subtree = CategoryTree.CategoriesInSubtree(categories, id.Value);
            var products = new List<ProductPreviewModel>();
            foreach (var categoryId in subtree)
            {
                products.AddRange(db.Product.Where(p => p.Category_id == categoryId).Select(ProductPreviewModel.FromProduct));
            }

            return products;
        }
    }
}
