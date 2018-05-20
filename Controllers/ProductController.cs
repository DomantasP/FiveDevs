using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Data;
using FiveDevsShop.Models;
using System.Net.Http;
using System.Threading.Tasks;
using FiveDevsShop.Services;

namespace FiveDevsShop.Controllers
{
    public class ProductController : Controller
    {
		private readonly ApplicationDbContext db;
        // Only one should be instantiated throughout the whole application
		private static readonly HttpClient client = new HttpClient();

        public ProductController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> GetProduct(int id)
        {
            var product = db.Product.FirstOrDefault(p => p.Id == id);


            CloudinaryClient.UploadImage();

            return View(product);
        }

        public IActionResult AddProduct(Product product)
        {            
            return View(product);
        }

//		public async IActionResult UploadImages()
//		{
//
//			
//
//            var responseString = await response.Content.ReadAsStringAsync();
//			return View();
//		}
    }
}