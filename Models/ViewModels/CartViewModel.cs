using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class CartViewModel
    {
        public ShoppingCart Cart { get; set; }
        public bool LoggedIn { get; set; }
    }
}
