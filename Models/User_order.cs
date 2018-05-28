using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models
{
    public class User_order
    {
        public int Id { get; set; }

        public string User_id { get; set; }

        public string date;

        public string Date
        {
            get { return date.Substring(0, 10); }
            set { date = value.ToString(); }
        }

        /*
         * 0 - Unconfirmed
         * 1 - Confirmed
         * 2 - Is sent out
         * 3 - Is delivered
         */
        public int Status { get; set; }

        public string Address { get; set; }

        public int Stars { get; set; }
        public String Comment { get; set; }

    }
}
