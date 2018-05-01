using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Data;

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
    }
}