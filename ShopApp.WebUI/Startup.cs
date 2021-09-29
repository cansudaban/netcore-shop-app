using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.WebUI.EmailServies;
using ShopApp.WebUI.Identity;
using System;

namespace ShopApp.WebUI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlite("Data Source=shopDb"));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new Microsoft.AspNetCore.Http.CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });

            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<ICartRepository, EfCoreCartRepository>();

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICartService, CartManager>();

            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
            new SmtpEmailSender(
                _configuration["EmailSender:Host"],
                _configuration.GetValue<int>("EmailSender:Port"),
                _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                _configuration["EmailSender:UserName"],
                _configuration["EmailSender:Password"])
            );

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "cart",
                    pattern: "cart",
                    defaults: new { controller = "Cart", action = "Index" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminusers",
                    pattern: "admin/user/list",
                    defaults: new { controller = "Admin", action = "UserList" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminuseredit",
                    pattern: "admin/user/{id?}",
                    defaults: new { controller = "Admin", action = "UserEdit" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminroles",
                    pattern: "admin/role/list",
                    defaults: new { controller = "Admin", action = "RoleList" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminroleedit",
                    pattern: "admin/role/{id?}",
                    defaults: new { controller = "Admin", action = "RoleEdit" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminrolecreate",
                    pattern: "admin/role/create",
                    defaults: new { controller = "Admin", action = "RoleCreate" }
                    );

                endpoints.MapControllerRoute(
                    name: "admincategorylist",
                    pattern: "admin/categories",
                    defaults: new { controller = "Admin", action = "CategoryList" }
                    );

                endpoints.MapControllerRoute(
                    name: "admincategorylist",
                    pattern: "admin/categories/{id?}",
                    defaults: new { controller = "Admin", action = "EditCategory" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminproductlist",
                    pattern: "admin/products",
                    defaults: new { controller = "Admin", action = "ProductList" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminproductlist",
                    pattern: "admin/products/{id?}",
                    defaults: new { controller = "Admin", action = "EditProduct" }
                    );

                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new { controller = "Shop", action = "search" }
                    );

                endpoints.MapControllerRoute(
                    name: "productdetails",
                    pattern: "{url}",
                    defaults: new { controller = "Shop", action = "Details" }
                    );

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new { controller = "Shop", action = "List" }
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });

            SeedIdentity.Seed(userManager,roleManager,configuration).Wait();
        }
    }
}
