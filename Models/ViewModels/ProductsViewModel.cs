using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class ProductsViewModel
    {
        [Required]
        public String idS { get; set; }

        [Required]
        public String sku_code { get; set; }

        [Required]
        public String categoryS { get; set; }

        [Required]
        public String title { get; set; }

        [Required]
        public String priceS { get; set; }

        [Required]
        public String description { get; set; }

        [Required]
        public String discountS { get; set; }

        [Required]
        public String subCategoryS { get; set; }

        public List<IFormFile> files { get; set; }



    }
}
