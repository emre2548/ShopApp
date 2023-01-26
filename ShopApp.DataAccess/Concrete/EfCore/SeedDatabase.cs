using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    /*
     static --> bir yerden alt özelliğe erişilebilir olmak için new lemek lazım onu yapmamak için
    ya da methodları bı şekilde alamamak için
    static hiç instance almadan içindeki herhangi bir özelliğe class ismi ile ulaşabilceğiz SeedDatabase.methodAdı gibi erişebileceğiz
     */
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            if (context.Database.GetPendingMigrations().Count()==0)
            {
                if (context.Categories.Count() == 0)
                {
                    // AddRange() birden fazla yapı eklememizi sağlar
                    context.Categories.AddRange(Categories); // yukarıodaki categories ile alakaso yok
                }
            }
            if (context.Products.Count() == 0)
            {
                context.Products.AddRange(Products);
                context.AddRange(ProductCategories);
            }
            context.SaveChanges();
        }

        private static Category[] Categories =
        {
            new Category(){Name="Telefon"},
            new Category(){Name="Bilgisayar"},
            new Category(){Name="Elektronik"}
        };

        private static Product[] Products =
        {
            new Product(){Name="Samsung Note5",Price=2000,ImageUrl="1.jpg",Description="<p> Güzel Telefon </p>"},
            new Product(){Name="Samsung Note6",Price=2000,ImageUrl="2.jpg",Description="<p>Güzel Telefon</p>"},
            new Product(){Name="Samsung Note7",Price=2000,ImageUrl="3.jpg",Description="<p> Güzel Telefon </p>"},
            new Product(){Name="Samsung Note8",Price=2000,ImageUrl="4.jpg",Description="<p>Güzel Telefon</p>"},
            new Product(){Name="Samsung Note9",Price=2000,ImageUrl="5.jpg",Description="<p> Güzel Telefon </p>"},
            new Product(){Name="Samsung Note10",Price=2000,ImageUrl="6.jpg",Description="<p> Güzel Telefon </p>"}
        };

        private static ProductCategory[] ProductCategories =
        {
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[1]},
            new ProductCategory(){Product=Products[2],Category=Categories[0]},
            new ProductCategory(){Product=Products[2],Category=Categories[2]},
            new ProductCategory(){Product=Products[3],Category=Categories[1]}
        };

    }
}
