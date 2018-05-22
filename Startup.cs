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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
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
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");


                routes.MapRoute(
                    name: "category",
                    template: "category/{id?}",
                    defaults: new { controller = "Category", action = "GetCategoryAndSubcategories" });
                
                routes.MapRoute(
                    name: "product",
                    template: "product/add",
                    defaults: new { controller = "Product", action = "AddProduct" });

                routes.MapRoute(
                    name: "get_product",
                    template: "product/{id?}",
                    constraints: new { id = new IntRouteConstraint() },
                    defaults: new { controller = "Product", action = "GetProduct" });
                
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
