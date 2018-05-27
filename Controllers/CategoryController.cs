﻿using FiveDevsShop.Data;
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
        const int ItemsPerPage = 20;

        private readonly ApplicationDbContext db;

        public CategoryController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult GetCategoryAndSubcategories(int? id, int page = 1)
        {
            Category current;
            IEnumerable<Category> subcategories = db.Category.Where(c => c.Parent_id == null).ToList();

            if (id == null)
            {
                current = null;
            }
            else
            {
                current = db.Category.Find(id);
            }

            return View(new CategoryViewModel()
            {
                Current = current,
                Subcategories = subcategories,
                Products = FindProductsInCategory(id, page),
            });
        }

        private ProductListViewModel FindProductsInCategory(int? id, int page)
        {
            // TODO: currently this loads all products fitting criteria

            IQueryable<Product> products = null;

            if (id == null)
            {
                // we are at root, return all products
                products = db.Product;
            }
            else
            {
                var categories = db.Category;
                var subtree = CategoryTree.CategoriesInSubtree(categories, id.Value);
                foreach (var categoryId in subtree)
                {
                    var newProducts = db.Product.Where(p => p.Category_id == categoryId);
                    if (products == null)
                        products = newProducts;
                    else
                        products = products.Union(newProducts);
                }
            }

            return Paging.LoadPage(products, page);
        }
    }
}
