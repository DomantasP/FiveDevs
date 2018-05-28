using System.Collections.Generic;

namespace FiveDevsShop.Models
{
    public class GetProductViewModel : Product
    { 
        public List<string> GalleryImagesUrls { get; set; }

        public string MainImageUrl { get; set; }

        public int ProductCount { get; set; } = 1;

        public List<Category> CategoryList { get; set; } = new List<Category>();
    }
}
