﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ProductPreviewModel> Products { get; set; }
    }
}