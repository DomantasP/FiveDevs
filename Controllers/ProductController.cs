using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Data;
using FiveDevsShop.Models;

namespace FiveDevsShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProductController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult GetProduct(int id)
        {
            var product = db.Product.Where(p => p.Id == id).FirstOrDefault();

            return View(product);
        }

        public IActionResult AddProduct(Product product)
        {
            
            return View(product);
        }
    }
}