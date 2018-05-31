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
            // implement order loading
            
            return View();
        }

        
        [HttpPost]
        public IActionResult ConfirmOrder(int orderId)
        {
            User_order order = db.User_order.FirstOrDefault(o => o.Id == orderId);

            //STATUS ENUM
            
            order.Status = 3;

            db.SaveChanges();

            return View("Orders");
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
