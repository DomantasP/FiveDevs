using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Models;
using FiveDevsShop.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

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
        public JsonResult GetSubcategoriesInBatches() //Id - order Id
        {
            var rootCategories = from c in db.Category
                                 where c.Parent_id == null
                                 select c;

            var categories = from c in db.Category
                             orderby c.Title
                             select c;

            var lookup = db.Category.ToLookup(x => x.Parent_id);
            var categoriesWithNoSub = (from rc in rootCategories
                                       join c in categories
                                       on rc.Id equals c.Parent_id into g
                                       select new
                                       {
                                           parentID = rc.Id,
                                           parentTitle = rc.Title,


                                           subCat = g.Select(w => new
                                                                  {
                                                                      subCatId = w.Id,
                                                                      subCatTitles = w.Title,
                                                                      subSubCat = from c in categories
                                                                                    where c.Parent_id == w.Id
                                                                                    select new
                                                                                    {
                                                                                        subSubCatId = c.Id,
                                                                                        subSubCatTitle = c.Title
                                                                                    }
                                                                  }
                                                             )

                                       }).OrderBy(t => t.parentTitle);

            Debug.WriteLine(lookup[1].Select(z => z.Id));
           


            return Json(categoriesWithNoSub.ToList());
        }
        [HttpPost]
        public JsonResult GetRootCategories() //Id - order Id
        {
            var rootCats = from r in db.Category.ToList()
                           where r.Parent_id == null
                           select new
                           {
                               id = r.Id,
                               title = r.Title
                           };

            return Json(rootCats);
        }

        [HttpPost]
        public JsonResult ConfirmOrder(int orderId) //Id - order Id
        {
            User_order order = db.User_order.FirstOrDefault(o => o.Id == orderId);
            Debug.WriteLine(orderId);
            
                order.Status += 1;

                try{db.SaveChanges();}
                catch{return null;}
     
            return Json(order.Id);
        }
        [HttpPost]
        public JsonResult GetOrderItems(int Id) //Id - order Id
        {
            var purchases = from p in db.Purchase
                            where p.Order_id == Id
                            select new
                            {
                                sku_code = p.Sku_code,
                                title = p.Title,
                                price = p.Price,
                                quantity = p.Quantity,
                                category = p.Category
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
                Status = order.Status,
                Stars = order.Stars,
                Comment = order.Comment
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

                user.Ban_flag = status;
                try{db.SaveChanges();}
                catch{return null;}
            
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
            var items = from i in db.Product.ToList()
                        join c in db.Category.ToList()
                        on i.CategoryId equals c.Id
                        select new
                        {
                            id = i.Id,
                            sku_code = i.SkuCode,
                            price = i.Price,
                            title = i.Title,
                            discount = i.Discount,
                            category = c.Title
                        };

            return Json(items);
        }

        [HttpPost]
        public IActionResult AdminGetProductById(int id)
        {
            ViewData["Message"] = "Redaguoti prekę";
            Product item = db.Product.First(p => p.Id == id);
            Category category = db.Category.First(c => c.Id == item.CategoryId);

            var itemWithNamedCategory =
            new
            {
                id = item.Id,
                sku_code = item.SkuCode,
                price = item.Price,
                title = item.Title,
                description = item.Description,
                discount = item.Discount,
                category = category.Title,
                categoryid = category.Id
            };
  
            return Json(itemWithNamedCategory);
        }
        
        [HttpPost]//ProductsViewModel product
        public IActionResult AdminUpdateProduct(ProductsViewModel product)//(String idS, String sku_code, String categoryS, String title, String priceS, String description, String discountS, String subCategoryS)
        {
            ViewData["Message"] = "Atnaujinti prekę";

            int id;
            decimal price;
            int discount;
            
            product.priceS = (product.priceS).Replace(",", ".");

            Debug.WriteLine(product.priceS + "Turi buti taskas");
      
            if (!Int32.TryParse(product.idS, out id)) return null;
            Product item = db.Product.FirstOrDefault(p => p.Id == id);
            if (item == null) return null;
            if (!Decimal.TryParse(product.priceS, NumberStyles.Number, CultureInfo.InvariantCulture, out price)) return null;
            //price = Convert.ToDecimal(product.priceS, new CultureInfo("en-US"));
            //price = decimal.Parse(product.priceS, new NumberFormatInfo() { NumberDecimalSeparator = "." });


            //if (!decimal.TryParse(product.priceS, out price)) return null;
            if (!Int32.TryParse(product.discountS, out discount)) return null;
            Debug.WriteLine(price);
           


            Category category;
            int setCategory = -1;
            category = db.Category.ToList().FirstOrDefault(c => c.Title == product.categoryS.Trim());
            if (category != null) setCategory = category.Id;

            if (category == null)
            {

                    category = new Category();
                    category.Title = product.categoryS.Trim();
                    category.Parent_id = null;
                    db.Category.Add(category);
                    try { db.SaveChanges(); }
                    catch { return null; }

                setCategory = (db.Category.ToList().FirstOrDefault(c => c.Title == product.categoryS.Trim())).Id;
            }
            Category subcategory = null;
            if (product.subCategoryS != null) subcategory = db.Category.ToList().FirstOrDefault(c => c.Title == product.subCategoryS.Trim());
            if(subcategory != null) setCategory = category.Id;
            if (product.subCategoryS != null && subcategory == null)
            {
                
                    Category newCategory = new Category();
                    newCategory.Title = product.subCategoryS.Trim();
                    newCategory.Parent_id = category.Id;
                    db.Category.Add(newCategory);
                    try { db.SaveChanges(); }
                    catch { return null; }
                    setCategory = (db.Category.ToList().FirstOrDefault(c => c.Title == product.subCategoryS.Trim())).Id;
                
            }

            
                    item.Id = id;
                    item.SkuCode = product.sku_code;
                    item.CategoryId = setCategory;
                    item.Title = product.title;
                    item.Price = price;
                    item.Description = product.description;
                    item.Discount = discount;

                try{db.SaveChanges();}
                catch{return null;}

                    return Json(item);
            
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
