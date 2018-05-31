using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.ManageViewModels
{
    public class PurchaseHistoryViewModel
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public int Status { get; set; }

        public List<Purchase> Items { get; set; }
    }
}
