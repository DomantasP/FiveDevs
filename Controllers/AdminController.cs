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
            List<Product> items = db.Product.ToList();
            ViewData["Message"] = "Administratoriaus pultas";

            return View(items);
        }
        public IActionResult UserManagementView()
        {
            ViewData["Message"] = "Naudotojų peržiūra";
            return PartialView();
        }
        public IActionResult SalesView()
        {
            ViewData["Message"] = "Pardavimų peržiūra";
            return PartialView();
        }
        public IActionResult AdminProductView()
        {
            ViewData["Message"] = "Prekių apžvalga";

            return PartialView();
        }
        public IActionResult AdminEditProductView()
        {
            List<Product> items = db.Product.ToList();
            ViewData["Message"] = "Redaguoti prekę";

            return PartialView(items);
        }
        [HttpPost]
        public JsonResult ConfirmOrder(int orderId) //Id - order Id
        {
            User_order order = db.User_order.FirstOrDefault(o => o.Id == orderId);
            Debug.WriteLine(orderId);
            using (var transaction = db.Database.BeginTransaction())
            {
                order.Status += 1;

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return null;
                }
                    transaction.Commit();
            }

            return Json(order.Id);
        }

        [HttpPost]
        public JsonResult GetOrderItems(int Id) //Id - order Id
        {
            //var purchases = db.Purchase.Select(p => p.Order_id == Id);

            var purchases = from p in db.Purchase
                            join i in db.Product
                            on p.Item_id equals i.Id
                            where p.Order_id == Id
                            select new
                            {
                                product_id = i.Id,
                                sku_code = i.Sku_code,
                                title = i.Title,
                                price = p.Price,
                                quantity = p.Quantity,
                                category = i.Category_id

                            };

            return Json(purchases);
        }
        [HttpPost]
        public JsonResult GetOrder(int Id)
        {
            User_order order = db.User_order.FirstOrDefault(o => o.Id == Id);
            ApplicationUser user = db.User.FirstOrDefault(u => u.Id == order.User_id);
            var cost = (from p in db.Purchase
                       where p.Order_id == order.Id
                       group p by new { p.Order_id } into pp
                       select pp.Sum(c => (c.Price * c.Quantity))).FirstOrDefault();

            var returnData = new
            {
                Id = order.Id,
                Date = order.Date,
                User = user.UserName,
                Address = order.Address,
                Cost = cost,
                Status = order.Status
            };
                             
            return Json(returnData);
        }

        
        [HttpPost]
        public JsonResult GetOrders(int status)
        {
            List<User_order> orders = db.User_order.ToList();
            List<Purchase> purchases = db.Purchase.ToList();
            List<ApplicationUser> users = db.User.ToList();

            var sortedOrders = from o in orders
                               where o.Status == status
                               select o;

            var sortedPurchase = from p in purchases
                                 join o in sortedOrders
                                 on p.Order_id equals o.Id
                                 join u in users
                                 on o.User_id equals u.Id
                                 group p by new {o.Id, o.Date, u.UserName} into pg
                                 select new
                                 {
                                     Id = pg.Key.Id,
                                     Date = pg.Key.Date,
                                     User = pg.Key.UserName,
                                     Cost = pg.Sum(c=>(c.Price * c.Quantity))
                                 };
            
  

            return Json(sortedPurchase);
        }

        // status to ban - 1; to unban - 0
        public JsonResult AdminChangeBanStatus(String username, int status)
        {
            ApplicationUser user = db.User.FirstOrDefault(p => p.UserName == username);


            using (var transaction = db.Database.BeginTransaction())
            {
                user.Ban_flag = status;

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return null;
                }
                transaction.Commit();
            }
            var userSpecialData = from c in db.User.ToList()
                                  where c.UserName == username
                                  select new
                                  {
                                      Username = c.UserName,
                                      Email = c.Email,
                                      Ban_flag = c.Ban_flag
                                  };

            return Json(userSpecialData);
        }

        [HttpPost]
        public IActionResult AdminBanUser(String username)
        {
            return AdminChangeBanStatus(username, 1);
        }

        [HttpPost]
        public IActionResult AdminUnbanUser(String username)
        {
            return AdminChangeBanStatus(username, 0);
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
            List<Product> items = db.Product.ToList();

            return Json(items);
        }

        [HttpPost]
        public IActionResult AdminGetProductById(int id)
        {
            Product item = db.Product.First(p => p.Id == id);
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
            Product item = db.Product.FirstOrDefault(p => p.Id == id);
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
