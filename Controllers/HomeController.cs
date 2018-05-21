<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
=======
﻿using System.Diagnostics;
using System.Linq;
>>>>>>> 08d56f08d5bd2ddc594122d1b251dd1800fd13f8
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Models;
using FiveDevsShop.Data;

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
<<<<<<< HEAD
    

=======
>>>>>>> 08d56f08d5bd2ddc594122d1b251dd1800fd13f8
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


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
