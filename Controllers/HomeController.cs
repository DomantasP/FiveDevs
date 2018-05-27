
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Models;
using FiveDevsShop.Data;
using FiveDevsShop.Models.DomainServices;

namespace FiveDevsShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult AdminMain()
        {
            ViewData["Message"] = "Add product page.";

            return View();
        }

        public IActionResult HomeProductList(int page = 1)
        {
            return View("Views/Home/Index.cshtml", new HomeViewModel()
            {
                Products = LoadProductPreviews(page),
            });
        }

        private ProductListViewModel LoadProductPreviews(int page)
        {
            return Paging.LoadPage(db.Product, page);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
