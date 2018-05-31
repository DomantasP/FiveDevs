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

        [Display(Name = "Vardas")]
        public string FirstName { get; set; }

        [Display(Name = "Pavardė")]
        public string LastName { get; set; }

        [Phone]
        [Display(Name = "Telefono numeris")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Miestas")]
        public string City { get; set; }

        [Display(Name = "Gatvė")]
        public string Street { get; set; }

        [Display(Name = "Namo numeris")]
        public string HouseNumber { get; set; }

        [Display(Name = "Buto numeris")]
        public string ApartmentNumber { get; set; }

        [Display(Name = "Pašto kodas")]
        public string PostalCode { get; set; }

        public string StatusMessage { get; set; }
    }
}
