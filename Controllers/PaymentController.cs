using FiveDevsShop.Data;
using FiveDevsShop.Extensions;
using FiveDevsShop.Models;
using FiveDevsShop.Models.Services.Payment;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly PaymentProcessor paymentProcessor;

        public PaymentController(ApplicationDbContext db, PaymentProcessor paymentProcessor)
        {
            this.db = db;
            this.paymentProcessor = paymentProcessor;
        }

        public IActionResult StartPayment()
        {
            var cart = this.UserShoppingCart();
            var amount = (int)Math.Round(cart.TotalCost() * 100);
            return View("/Views/User/Payment.cshtml", new PaymentViewModel()
            {
                Amount = amount,
                ShoppingCart = JsonConvert.SerializeObject(cart),
            });
        }

        [HttpPost]
        public IActionResult Pay(PaymentViewModel args)
        {
            if (!ModelState.IsValid)
                return View("/Views/User/Payment.cshtml", args);

            var cart = JsonConvert.DeserializeObject<ShoppingCart>(args.ShoppingCart);

            using (var transaction = db.Database.BeginTransaction())
            {
                var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
                var orderEntry = db.User_order.Add(new User_order()
                {
                    User_id = "?",
                    Date = date,
                    Status = 0,
                    Address = "?",
                    Stars = 0,
                    Comment = "",
                });
                db.SaveChanges();
                var totalPrice = cart.TotalCost();
                foreach (var entry in cart.Entries.Values)
                {
                    db.Purchase.Add(new Purchase()
                    {
                        Order_id = orderEntry.Entity.Id,
                        Sku_code = entry.SkuCode,
                        Quantity = entry.Amount,
                        Price = entry.FinalPrice,
                        Title = entry.Title,
                        Category = entry.Category,
                    });
                }

                //try
                //{
                paymentProcessor.Pay(new PaymentData()
                {
                    Amount = (int)Math.Round(totalPrice * 100),
                    CardNumber = args.Number,
                    Holder = args.Name,
                    ExpirationYear = args.ExpYear,
                    ExpirationMonth = args.ExpMonth,
                    Cvv = args.CCV,
                });

                    db.SaveChanges();
                    transaction.Commit();
                /*}
                catch (Exception e)
                {
                   Debug.WriteLine("Payment failed:");
                    Debug.WriteLine(e);
                    transaction.Rollback();
                }*/
            }

            return View("/Views/User/PaymentDone.cshtml");
        }
    }
}
