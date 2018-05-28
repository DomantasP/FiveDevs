using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Vartotojo vardas")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "El. paštas")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Slaptažodis")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Pakartokite slaptažodį")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Vardas")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Pavardė")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Miestas")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Gatvė")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Namo numeris")]
        public int HouseNumber { get; set; }

        [Display(Name = "Buto numeris")]
        public int? ApartmentNumber { get; set; }

        [Required]
        [Display(Name = "Pašto kodas")]
        public string PostalCode { get; set; }
    }
}
