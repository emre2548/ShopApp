using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete
{
    public class ShopContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ShopApp;Trusted_Connection=true");
            // DB localde oluşması için
            //optionsBuilder.UseSqlServer(@"Server=localhost;Database=ShopAppHI;integrated security = true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // CAtegoryID ve ProductID birlikite bir primary key oluştururlar
            // aynı categoryId ile ProductId birden fazla kez eşleşmesini engelledik
            modelBuilder.Entity<ProductCategory>()
                .HasKey(c => new { c.CategoryId, c.ProductId });
        }

        public DbSet<Product> Products { get; set; }  //Pluraize Her class adı sonuna 's' ekler.
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        //public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }


        /*
         Terminal Ekranına Sırayla
        1- DataAccess katmanını terminal ekranında aç
        2- dotnet ef database drop  --> silme yapar
        dotnet ef migrations add CreateDatabase  --> migration oluştur
        dotnet-ef database update --> veritabanı oluştur
         */

    }
}
