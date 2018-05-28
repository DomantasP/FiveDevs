﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FiveDevsShop.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //public string Id { get; set; }

        /*[Column("UserName")]
        public string Username { get; set; }*/

        //public string Email { get; set; }

        public int Ban_flag { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int HouseNumber { get; set; }

        public int? ApartmentNumber { get; set; }

        public string PostalCode { get; set; }
    }
}
