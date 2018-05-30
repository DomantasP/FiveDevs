using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Vartotojo vardas")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "El. Paštas")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefono numeris")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
