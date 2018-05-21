using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveDevsShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? Parent_id { get; set; }
    }
}
