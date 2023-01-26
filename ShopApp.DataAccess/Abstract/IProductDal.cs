using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Abstract
{
    public interface IProductDal : IRepository<Product>
    {
        //IEnumerable<Product> GetPopularProducts();

        List<Product> GetProductsByCategory(string category,int page,int pageSize);

        Product GetProductDetails(int id);
        int GetCountByCategory(string category);
        Product GetByIdWithCategories(int id);
        void Update(Product entity, int[] categoryIds);
        void Create(Product entity, int[] categoryIds);
    }
}
