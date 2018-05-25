using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<Product> Product { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<User_order> User_order { get; set; }
        public DbSet<Image> Image { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Purchase>().HasKey(c => new { c.Order_id, c.Item_id });
        }
            
        

    }
}
