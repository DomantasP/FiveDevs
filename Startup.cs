using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FiveDevsShop.Data;
using FiveDevsShop.Models;
using FiveDevsShop.Services;
using Microsoft.AspNetCore.Routing.Constraints;
using FluentValidation.AspNetCore;
using FiveDevsShop.Validators;
using FluentValidation;
using FiveDevsShop.Models.AccountViewModels;

namespace FiveDevsShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BuildAppSettingsProvider();
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                var allowed = options.User.AllowedUserNameCharacters + "ĄąČčĘęĖėĮįŠšŲųŪūŽž";
                options.User.AllowedUserNameCharacters = allowed;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc().AddFluentValidation();

            services.AddTransient<IValidator<GetProductViewModel>, GetProductViewModelValidator>();
            services.AddTransient<IValidator<RegisterViewModel>, RegisterViewModelValidator>();
            services.AddTransient<IValidator<LoginViewModel>, LoginViewModelValidator>();
            services.AddTransient<IValidator<ForgotPasswordViewModel>, ForgotPasswordViewModelValidator>();
            services.AddTransient<IValidator<AddProductViewModel>, AddProductViewModelValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "home",
                    template: "",
                    defaults: new { controller = "Category", action = "GetCategoryAndSubcategories" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "category",
                    template: "category/{id?}",
                    defaults: new { controller = "Category", action = "GetCategoryAndSubcategories" });
                
                routes.MapRoute(
                    name: "product_add",
                    template: "product/add",
                    defaults: new { controller = "Product", action = "AddProductView" });
                
                routes.MapRoute(
                    name: "product_edit",
                    template: "product/edit/{id?}",
                    constraints: new { id = new IntRouteConstraint() },
                    defaults: new { controller = "Product", action = "EditProductView" });

                routes.MapRoute(
                    name: "product_delete",
                    template: "product/delete/{id?}",
                    constraints: new { id = new IntRouteConstraint() },
                    defaults: new { controller = "Product", action = "DeleteProduct" });

                routes.MapRoute(
                    name: "get_product",
                    template: "product/{id?}",
                    constraints: new { id = new IntRouteConstraint() },
                    defaults: new { controller = "Product", action = "GetProduct" });

                routes.MapRoute(
                    name: "index",
                    template: "index",
                    defaults: new { controller = "Home", action = "HomeProductList" });
                                
                routes.MapRoute(
                    name: "admin",
                    template: "admin",
                    defaults: new { controller = "Admin", action = "AdminMain" });
                
                routes.MapRoute(
                    name: "admin_categories",
                    template: "admin/categories",
                    defaults: new { controller = "Admin", action = "Categories" });
                
                routes.MapRoute(
                    name: "admin_orders",
                    template: "admin/orders",
                    defaults: new { controller = "Admin", action = "Orders" });
                
                routes.MapRoute(
                    name: "admin_users",
                    template: "admin/users",
                    defaults: new { controller = "Admin", action = "Users" });
                
                routes.MapRoute(
                    name: "lockout",
                    template: "admin/lockout-user/{id?}",
                    defaults: new { controller = "Admin", action = "LockoutUser" });
                
                routes.MapRoute(
                    name: "unlock",
                    template: "admin/unlock-user/{id?}",
                    defaults: new { controller = "Admin", action = "UnlockUser" });
            });
        }

        private void BuildAppSettingsProvider()
        {
            AppSettingsProvider.CloudinaryCloud = Configuration["CloudinaryCredentials:Cloud"];
            AppSettingsProvider.CloudinaryApiKey = Configuration["CloudinaryCredentials:ApiKey"];
            AppSettingsProvider.CloudinarytSecret = Configuration["CloudinaryCredentials:Secret"];
        }
    }
}
