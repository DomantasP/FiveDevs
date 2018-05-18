using FiveDevsShop.Data;
using FiveDevsShop.Models;
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
            FiveDevsShop.Models.Category category;
            List<Category> subCategories = new List<Category>();

            //The first element in subcategories list is a category and the later ones are its subcategories.
            if(id == null)
            {
                subCategories = db.Category.Where(c => c.Parent_id == null).ToList();
                subCategories.Insert(0, null);
            }
            else
            {
                category = db.Category.Find(id);
                subCategories = db.Category.Where(c => c.Parent_id == id).ToList();
                subCategories.Insert(0, category);
            }

            return View(subCategories);
        }
    }
}
