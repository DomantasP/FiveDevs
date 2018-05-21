using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class ProductViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }
        
        [Required]
        public int Discount { get; set; }
        
        [Required] public string SkuCode { get; set; }
        
    }
}
