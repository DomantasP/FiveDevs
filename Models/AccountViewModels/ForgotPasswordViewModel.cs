﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "El. paštas")]
        public string Email { get; set; }
    }
}
