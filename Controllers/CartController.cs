using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiveDevsShop.Extensions;
using FiveDevsShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveDevsShop.Controllers
{
    public class CartController : Controller
    {
        
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(UserManager<ApplicationUser> um)
        {
           
            userManager = um;
        }
        public async Task<IActionResult> ViewCart()
        {
            var user = await userManager.GetUserAsync(User);
            var cart = this.UserShoppingCart();
            return View("/Views/User/Cart.cshtml", new CartViewModel()
            {
                Cart = cart,
                LoggedIn = user != null,
            });
        }
        public async Task<IActionResult> RemoveProduct(CartEntry entry)
        {
            var cart = this.UserShoppingCart();      
            cart.removeFromCart(entry.SkuCode);
            this.SaveCart(cart);
            return await ViewCart();
        }
        public async Task<IActionResult> AddOne(CartEntry entry)
        {
            var cart = this.UserShoppingCart();
            cart.increaseAmountByOne(entry.SkuCode);
            this.SaveCart(cart);
            return await ViewCart();
        }
        public async Task<IActionResult> RemoveOne(CartEntry entry)
        {
            var cart = this.UserShoppingCart();
            cart.decreaseAmountByOne(entry.SkuCode);
            this.SaveCart(cart);
            return await ViewCart();
        }

    }
}