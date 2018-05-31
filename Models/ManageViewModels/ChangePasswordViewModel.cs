using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Dabartinis slaptažodis")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Naujas slaptažodis")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Pakartokite naują slaptažodį")]
        [Compare("NewPassword", ErrorMessage = "Nauji slaptažodžiai nesutampa.")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
