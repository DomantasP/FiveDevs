using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Models;
using FiveDevsShop.Data;

namespace FiveDevsShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;

        public AdminController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult AdminMain()
        {
            List<Item> items = db.Item.ToList();
            ViewData["Message"] = "Administratoriaus pultas";

            return View(items);
        }
        public IActionResult UserManagementView()
        {
            ViewData["Message"] = "Naudotojų peržiūra";
            return PartialView();
        }
        public IActionResult AdminProductView()
        {
            ViewData["Message"] = "Prekių apžvalga";

            return PartialView();
        }
        public IActionResult AdminEditProductView()
        {
            List<Item> items = db.Item.ToList();
            ViewData["Message"] = "Redaguoti prekę";

            return PartialView(items);
        }
        [HttpPost]
        public IActionResult AdminGetUserModel()
        {
            List<ApplicationUser> users = db.User.ToList();
            var usersAdminData =  from c in users
                select new { Username = c.UserName,
                            Email = c.Email,
                            Ban_flag = c.Ban_flag };

            return Json(usersAdminData);
        }
        [HttpPost]
        public IActionResult AdminGetModel()
        {
            List<Item> items = db.Item.ToList();

            return Json(items);
        }

        [HttpPost]
        public IActionResult AdminGetProductById(int id)
        {
            Item item = db.Item.First(p => p.Id == id);
            ViewData["Message"] = "Redaguoti prekę";

            return Json(item);
        }

        [HttpPost]
        public IActionResult AdminUpdateProduct(String idS, String sku_code, String category_idS, String title, String priceS, String description, String discountS)
        {
            ViewData["Message"] = "Atnaujinti prekę";

            int id;
            int category_id;
            decimal price;
            int discount;
            priceS = priceS.Replace(",", ".");

            if (!Int32.TryParse(idS, out id)) return null;
            Item item = db.Item.FirstOrDefault(p => p.Id == id);
            if (item == null) return null;
            if (!Int32.TryParse(category_idS, out category_id)) return null;
            if (!Decimal.TryParse(priceS, NumberStyles.Number, CultureInfo.InvariantCulture, out price)) return null;
            if (!Int32.TryParse(discountS, out discount)) return null;
            using (var transaction = db.Database.BeginTransaction())
            {
                    item.Id = id;
                    item.Sku_code = sku_code;
                    item.Category_id = category_id;
                    item.Title = title;
                    item.Price = price;
                    item.Description = description;
                    item.Discount = discount;

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return null;
                }
                    transaction.Commit();
                    return Json(item);
            }  

        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
