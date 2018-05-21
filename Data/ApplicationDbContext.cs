﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FiveDevsShop.Models;

namespace FiveDevsShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Item> Item { get; set; }

        public DbSet<Image> Image { get; set; }

    }
}
