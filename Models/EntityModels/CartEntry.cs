using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    [Serializable]
    public class CartEntry
    {
        public Product Product { get; set; }
        public int Amount { get; set; }
    }
}
