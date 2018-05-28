using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FiveDevsShop.Models
{
    public class AddProductViewModel : Product
    {
        public List<IFormFile> Images { get; set; }

        public IFormFile MainImageFile { get; set; }

        public List<Category> Categories { get; set; }
    }
}
