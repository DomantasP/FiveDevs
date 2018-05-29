using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Vartotojo vardas")]
        public string UserName { get; set; }

        [Display(Name = "El. paštas")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Slaptažodis")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Pakartokite slaptažodį")]
        [Compare("Password", ErrorMessage = "Slaptažodžiai nesutampa.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Vardas")]
        public string FirstName { get; set; }

        [Display(Name = "Pavardė")]
        public string LastName { get; set; }

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
    }
}
