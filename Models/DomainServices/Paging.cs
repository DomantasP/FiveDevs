using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.DomainServices
{
    public class Paging
    {
        // TODO: this should be changeable by administrator
        public const int ItemsPerPage = 16;

        public static ProductListViewModel LoadPage(IQueryable<Product> products, int page)
        {
            var productCount = products.Count();
            int pages = Math.Max(1, (productCount + ItemsPerPage - 1) / ItemsPerPage);
            page = Math.Clamp(page, 1, pages);

            var productsInPage = products.Skip((page - 1) * ItemsPerPage).Take(ItemsPerPage).Select(ProductPreviewModel.FromProduct);

            return new ProductListViewModel()
            {
                Products = productsInPage,
                CurrentPage = page,
                PageCount = pages,
            };
        }
    }
}
