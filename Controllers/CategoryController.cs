using FiveDevsShop.Data;
using FiveDevsShop.Models;
using FiveDevsShop.Models.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace FiveDevsShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly Paging paging;

        public CategoryController(ApplicationDbContext db, Paging paging)
        {
            this.db = db;
            this.paging = paging;
        }

        public IActionResult GetCategoryAndSubcategories(int? id, int page = 1)
        {
            Category current;

            var tree = new CategoryTree(db.Category);

            if (id == null)
                current = null;
            else
            {
                current = tree.FindCategoryNode(id.Value)?.Category;
                if (current == null)
                    return View("NotFound");
            }

            var subtrees = tree.Subtrees(id).ToList();
            var products = ShowProductsInCategory(tree, id, page);

            return View(new CategoryViewModel()
            {
                CategoryPath = tree.FindPath(current),
                Subtrees = subtrees,
                Products = products,
            });
        }

        private ProductListViewModel ShowProductsInCategory(CategoryTree tree, int? id, int page)
        {
            return paging.LoadPage(FindProductsInCategory(tree, id), page);
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
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCategory(CategoryDeleteViewModel model)
        {
            if (db.Product.Any(p => p.CategoryId == model.Id))
            {
                return RedirectToAction("Categories","Admin");
            }
            
            var category = db.Category.FirstOrDefault(c=> c.Id == model.Id);
            
            if (category == null) return null;
            
            //Find all categories to delete
            List<Category> allCategories = new List<Category>();
            allCategories.Add(category);
            for(int i = 0; i < allCategories.Count; i++)
            {
                Category certainCat = allCategories[i];
                List<Category> tempCatList = db.Category.Where(c => c.Parent_id == certainCat.Id).ToList();
                allCategories.AddRange(tempCatList);
            }

            //Delete categories
            foreach (var c in allCategories)
            {
                Product product = db.Product.FirstOrDefault(p => p.CategoryId == c.Id);
                //if(product != null) Debug.WriteLine(product.Title);
                //means that category or its subcat is assigned to some item(s)
                if (product != null) return null;
            }
            db.Category.RemoveRange(allCategories);
            db.SaveChanges();

            return RedirectToAction("Categories","Admin");
        }
        
        public IActionResult AddCategory(CategoryAddViewModel model)
        {

            db.Category.Add(new Category()
            {
                Parent_id = model.ParentId,
                Title = model.Title
            });

            db.SaveChanges();

            return RedirectToAction("Categories", "Admin");
        }
    }
}
