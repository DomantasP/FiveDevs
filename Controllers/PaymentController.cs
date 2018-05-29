using FiveDevsShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Controllers
{
    public class PaymentController : Controller
    {
        public PaymentController()
        {

        }

        public IActionResult StartPayment()
        {
            return View("/Views/User/Payment.cshtml", new PaymentViewModel());
        }

        [HttpPost]
        public IActionResult Pay(PaymentViewModel args)
        {
            /*if (!ModelState.IsValid)
            {
                return View("/Views/User/Payment.cshtml", args);
            }*/
            return View("/Views/Home/Index.cshtml");
        }
    }
}
