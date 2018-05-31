using System;
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
using FiveDevsShop.Models.ManageViewModels;
using System.Net.Http;
using FiveDevsShop.Models.Services.Payment;
using FiveDevsShop.Models.DomainServices;

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

            services.AddSingleton(new HttpClient());

            services.AddMvc().AddFluentValidation().AddSessionStateTempDataProvider();

            services.AddTransient<IValidator<GetProductViewModel>, GetProductViewModelValidator>();
            services.AddTransient<IValidator<RegisterViewModel>, RegisterViewModelValidator>();
            services.AddTransient<IValidator<LoginViewModel>, LoginViewModelValidator>();
            services.AddTransient<IValidator<ForgotPasswordViewModel>, ForgotPasswordViewModelValidator>();
            services.AddTransient<IValidator<IndexViewModel>, IndexViewModelValidator>();
            services.AddTransient<IValidator<ChangePasswordViewModel>, ChangePasswordViewModelValidator>();
            services.AddTransient<IValidator<PaymentViewModel>, PaymentViewModelValidator>();
            services.AddTransient<IValidator<CategoryAddViewModel>, CategoryAddViewModelValidator>();
            services.AddTransient<PriceCalculator>();
            services.AddTransient<IPaymentProcessor, PaymentProcessor>();
            services.AddTransient<IImageUploader, CloudinaryClient>();
            services.AddTransient<Paging>();
            
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
            });
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
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "payment",
                    template: "payment",
                    defaults: new { controller = "Payment", action = "StartPayment" });

                routes.MapRoute(
                    name: "cart",
                    template: "cart",
                    defaults: new { controller = "Product", action = "ViewCart" });

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
                    name: "admin_product",
                    template: "admin/product",
                    defaults: new { controller = "Admin", action = "Product" });
                
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
                
                routes.MapRoute(
                    name: "confirm",
                    template: "admin/confirm-order/{id?}",
                    defaults: new { controller = "Admin", action = "UpdateOrderStatus" });
                
                routes.MapRoute(
                    name: "send",
                    template: "admin/send-order/{id?}",
                    defaults: new { controller = "Admin", action = "UpdateOrderStatus" });
                
                routes.MapRoute(
                    name: "ship",
                    template: "admin/ship-order/{id?}",
                    defaults: new { controller = "Admin", action = "UpdateOrderStatus" });
                
                routes.MapRoute(
                    "NotFound",
                    "{*url}",
                    new { controller = "Home", action = "NotFound" }
                );
            });
        }

        private void BuildAppSettingsProvider()
        {
            AppSettingsProvider.CloudinaryCloud = Configuration["CloudinaryCredentials:Cloud"];
            AppSettingsProvider.CloudinaryApiKey = Configuration["CloudinaryCredentials:ApiKey"];
            AppSettingsProvider.CloudinarytSecret = Configuration["CloudinaryCredentials:Secret"];
            AppSettingsProvider.PaymentUsername = Configuration["PaymentCredentials:Username"];
            AppSettingsProvider.PaymentPassword = Configuration["PaymentCredentials:Password"];
            AppSettingsProvider.PaymentServiceUrl = Configuration["PaymentCredentials:Url"];
        }
    }
}
