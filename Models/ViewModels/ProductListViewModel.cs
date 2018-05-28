using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductPreviewModel> Products { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public string QueryParams { get; private set; } = "";

        public string MakePageUrl(int pageNumber)
        {
            if (QueryParams != "")
                return $"?page={pageNumber}&{QueryParams}";
            else
                return $"?page={pageNumber}";
        }

        public void AddQueryParam(string key, string value)
        {
            value = System.Net.WebUtility.HtmlEncode(value);
            if (QueryParams == "")
            {
                QueryParams = $"{key}={value}";
            }
            else
            {
                QueryParams += $"&{key}={value}";
            }
        }
    }
}
