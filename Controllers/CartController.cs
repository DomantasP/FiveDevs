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
        private readonly ShoppingCart shoppingCart;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ShoppingCart sc, UserManager<ApplicationUser> um)
        {
            shoppingCart = sc;
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
        public RedirectToActionResult RemoveProduct(string skuCode)
        {
            shoppingCart.removeFromCart(skuCode);
            this.SaveCart(shoppingCart);
            return RedirectToAction("ViewCart");
        }
        
    }
}