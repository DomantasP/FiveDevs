using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class PaymentViewModel
    {
        public string ShoppingCart { get; set; }
        public int Amount { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public int ExpYear { get; set; }
        public int ExpMonth { get; set; }
        public string CCV { get; set; }

        public string ErrorMessage { get; set; }
    }
}
