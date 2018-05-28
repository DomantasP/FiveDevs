using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class ProductsImportModel
    {
        [Required]
        public String SkuCode { get; set; }

        [Required]
        public List<String> Categories { get; set; }

        [Required]
        public String Title { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public String ShortDescription { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public int Discount { get; set; } = 0;

        public List<String> Images { get; set; }

        public List<ProductProperty> Properties { get; set; }



    }
}
