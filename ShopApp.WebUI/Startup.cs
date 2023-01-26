using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUI.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopApp.WebUI.Identity;

namespace ShopApp.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRazorPages();
            //services.AddScoped<IProductDal, MemoryProductDal>();

            // kullanýcý iþlemleri
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ShopApp;Trusted_Connection=true"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //Password Özellikleri
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;

                options.Lockout.MaxFailedAccessAttempts = 5; //Max 5 defa yanlýþ giriþ hakký
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //5 kereden sonra 5 dakika giriþ yasak
                options.Lockout.AllowedForNewUsers = true; //Yeni üyelere de uygula


                //options.User.AllowedUserNameCharacters = ""; //Username de ý kullanma
                options.User.RequireUniqueEmail = true; //Ayný Email Adresi ile Kayýt Engelle

                options.SignIn.RequireConfirmedEmail = true; //Mail ile aktive etme açýk
                options.SignIn.RequireConfirmedPhoneNumber = false; //Telefondan active etme 
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";   //Giriþ Sayfasý
                options.LogoutPath = "/account/logout"; //Çýkýþ Sayfasý
                options.AccessDeniedPath = "/account/accessdenied"; //Yetkisiz sayfaya enter yönlen. Action
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //Cookie süresi(default 20dk)
                options.SlidingExpiration = true; //Tarayýcý scriptlerine izin 
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });

            services.AddScoped<IProductDal, EfCoreProductDal>();
            services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
            services.AddScoped<ICartDal, EfCoreCartDal>();
            services.AddScoped<IOrderDal, EfCoreOrderDal>();

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // kendimiz ekledik
                SeedDatabase.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            /* ----------------------------------------------------------------------------------------------------
             *
             *                                      BURADA SIRALAMA ÖNEMLÝ
             *
             * ----------------------------------------------------------------------------------------------------
             */
            //app.UseStaticFiles();

            app.UseStaticFiles(); // tüm static dosyalarý aktif et

            app.CustomStaticFiles();

            app.UseAuthentication(); // oturum açma kýsmý için

            app.UseRouting();

            app.UseAuthorization();

            //app.UseStaticFiles(); // tüm static dosyalarý aktif et

            //app.CustomStaticFiles();

            //app.UseAuthentication(); // oturum açma kýsmý için

            //eski varsiyonda core 3.1
            //app.UseMap(routes =>
            //{
            //     defaults=..
            //     templates={Controllers}/{Actions}/{id}
            //     route=Controller="Home",Action="Index",?id
            //});

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new { controller = "Shop", action = "List" });

                endpoints.MapControllerRoute(
                    name: "adminProducts",
                    pattern: "admin/products",
                    defaults: new { controller = "Admin", action = "ProductList" });

                endpoints.MapControllerRoute(
                    name: "adminProducts",
                    pattern: "admin/products/{id?}",
                    defaults: new { controller = "Admin", action = "EditProduct" });

                endpoints.MapControllerRoute(
                    name: "adminCategories",
                    pattern: "admin/category/{id?}",
                    defaults: new { controller = "Admin", action = "EditCategory" });

                endpoints.MapControllerRoute(
                    name: "cart",
                    pattern: "cart",
                    defaults: new { controller = "Cart", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "checkout",
                    pattern: "checkout",
                    defaults: new { controller = "Cart", action = "Checkout" });

                endpoints.MapControllerRoute(
                    name: "orders",
                    pattern: "orders",
                    defaults: new { controller = "Cart", action = "GetOrders" });
            });

            SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();
        }
    }
}
