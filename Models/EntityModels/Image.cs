using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class Image
    {
        public string Id { get; set; }
        
        public int ProductId { get; set; }

        public string Url { get; set; }
    }
}
