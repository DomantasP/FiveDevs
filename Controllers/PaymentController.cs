using FiveDevsShop.Data;
using FiveDevsShop.Extensions;
using FiveDevsShop.Models;
using FiveDevsShop.Models.Services.Payment;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> userManager;

        public PaymentController(ApplicationDbContext db, PaymentProcessor paymentProcessor, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.paymentProcessor = paymentProcessor;
            this.userManager = userManager;
        }

        public async Task<IActionResult> StartPayment()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("/Views/Account/AccessDenied.cshtml");
            }

            var cart = this.UserShoppingCart();
            var amount = (int)Math.Round(cart.TotalCost() * 100);
            return View("/Views/User/Payment.cshtml", new PaymentViewModel()
            {
                Amount = amount,
                ShoppingCart = JsonConvert.SerializeObject(cart),
            });
        }

        [HttpPost]
        public async Task<IActionResult> Pay(PaymentViewModel args)
        {
            args.ErrorMessage = null;
            if (!ModelState.IsValid)
                return View("/Views/User/Payment.cshtml", args);

            var cart = JsonConvert.DeserializeObject<ShoppingCart>(args.ShoppingCart);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("/Views/Account/AccessDenied.cshtml");
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                string address;
                if (!string.IsNullOrEmpty(user.ApartmentNumber))
                    address = $"{user.Street} g. {user.HouseNumber}-{user.ApartmentNumber}, LT-{user.PostalCode} {user.City}";
                else
                    address = $"{user.Street} g. {user.HouseNumber}, LT-{user.PostalCode} {user.City}";

                var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
                var orderEntry = db.User_order.Add(new User_order()
                {
                    User_id = user.Id,
                    Date = date,
                    Status = 0,
                    Address = address,
                    Stars = -1,
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

                try
                {
                    await paymentProcessor.Pay(new PaymentData()
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
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    args.ErrorMessage = $"Blogi duomenys: {e.Message}";
                    return View("/Views/User/Payment.cshtml", args);
                }
                catch (CardExpiredException e)
                {
                    transaction.Rollback();
                    args.ErrorMessage = "Nepavyko apmokėti: kortelės galiojimo laikas baigėsi";
                    return View("/Views/User/Payment.cshtml", args);
                }
                catch (OutOfFundsException e)
                {
                    transaction.Rollback();
                    args.ErrorMessage = "Nepavyko apmokėti: nepakanka pinigų";
                    return View("/Views/User/Payment.cshtml", args);
                }
                catch (UnknownErrorException e)
                {
                    transaction.Rollback();
                    args.ErrorMessage = $"Nepavyko apmokėti: nežinoma klaida: {e.Message}";
                    return View("/Views/User/Payment.cshtml", args);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    args.ErrorMessage = $"Nepavyko apmokėti: vidinė klaida: {e.Message}";
                    return View("/Views/User/Payment.cshtml", args);
                }
            }

            return View("/Views/User/PaymentDone.cshtml");
        }
    }
}
