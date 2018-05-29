using FiveDevsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace FiveDevsShop.Extensions
{
    public static class ControllerExtensions
    {
        private const string Key = "shopping-cart";

        public static ShoppingCart UserShoppingCart(this Controller controller)
        {

            if (!controller.TempData.ContainsKey(Key))
                controller.TempData.Put(Key, new ShoppingCart());

            var cart = controller.TempData.Get<ShoppingCart>(Key);
            controller.TempData.Keep(Key);

            return cart;
        }

        public static void SaveCart(this Controller controller, ShoppingCart cart)
        {
            controller.TempData.Put(Key, cart);
        }
    }
}
