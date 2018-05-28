using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FiveDevsShop.Models
{
    public class Purchase
    {
        /*[Key]
        [Column(Order = 1)]*/
        public int Order_id { get; set; }

        /*[Key]
        [Column(Order = 2)]*/
        public String Sku_code { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public String Title { get; set; }

        public String Category { get; set; }

    }
}
