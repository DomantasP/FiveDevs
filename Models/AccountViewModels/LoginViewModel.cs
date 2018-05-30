using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "El. paštas")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Slaptažodis")]
        public string Password { get; set; }

        [Display(Name = "Prisiminti mane")]
        public bool RememberMe { get; set; }
    }
}
