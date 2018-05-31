using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Models;
using FiveDevsShop.Data;
using FiveDevsShop.Models.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace FiveDevsShop.Controllers
{        
    
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }

        public IActionResult AdminMain()
        {
            List<Product> items = db.Product.ToList();

            return View(items);
        }
        
        public IActionResult Categories()
        {
            var tree = new CategoryTree(db.Category);

            var subtrees = tree.Subtrees(null).ToList();

            return View(new CategoryViewModel()
            {
                CategoryPath = tree.FindPath(null),
                Subtrees = subtrees
            });
        }
       
        
        public IActionResult Product(int page = 1)
        {
            var model = Paging.LoadPage(db.Product, page);
            
            return View(model);
        }

        public IActionResult Orders()
        {
            var orders = db.User_order
                .Select(m => new OrderViewModel()
                {
                    Id = m.Id,
                    Date = m.Date,
                    Status = m.Status
                })
                .OrderBy(m => m.Date)
                .ToList();


            orders.ForEach(order => 
                order.Items = db.Purchase
                    .Where(p => p.Order_id == order.Id)
                    .ToList()
                );
            
            return View(orders);
        }

        public IActionResult UpdateOrderStatus(int id)
        {
            var order = db.User_order.Find(id);
            
            if(order.Status > 3)
                return RedirectToAction("Orders");

            order.Status++;
            db.SaveChanges();

            return RedirectToAction("Orders");
        }
        
        public IActionResult Users()
        {
            var model = _userManager.Users.ToList();
            
            return View(model);
        }
        
        public async Task<RedirectToActionResult> LockoutUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var model = _userManager.SetLockoutEnabledAsync(user,true);
            
            return RedirectToAction("Users");
        }
        
        public async Task<RedirectToActionResult> UnlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var model = _userManager.SetLockoutEnabledAsync(user, false);
            
            return RedirectToAction("Users");
        }
    }
}
