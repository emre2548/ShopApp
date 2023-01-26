using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entities
{
    /* bu bizim ara tablomuz iki tabloyu çoka çok ilişki ile bağlamak için kullanacağız */
    public class ProductCategory
    {
        //public int Id { get; set; } // bunu primary key göreceği için sorun olmasın diye kaldırdık
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

