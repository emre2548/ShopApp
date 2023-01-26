using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.DataAccess.Concrete
{
    public class EfCoreOrderDal : EfCoreGenericRepository<Order, ShopContext>, IOrderDal
    {
        public List<Order> GetOrders(string userId) // nullable yapmadık Null gelme durumuda var
        {
            using (var context = new ShopContext())
            {
                var orders = context.Orders
                    .Include(i => i.OrderItems)
                    .ThenInclude(i => i.Product)
                    .AsQueryable();

                /*
                 * AsQueryable() ifadesi sorguyu sonradan değiştirebilmemizi ve yapılan sorgu
                 * işleminin SQL (DB) tarafına yükler
                 * AsQueryable() ifadesi ile sorgu userId ye göre çekilir
                 * userId gelirse sadece kullanıcıya ait siparişler
                 */
                if (!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(i => i.UserId == userId);
                }

                /*
                 * userId gelmez ise bütün siparişleri getir siparişlere bakan Admin demektir
                 */

                return orders.ToList();
            }
        }

        public List<Order> GetOrders()
        {
            using (var context = new ShopContext())
            {
                var orders = context.Orders
                    .Include(i => i.OrderItems)
                    .ThenInclude(i => i.Product).ToList();

                return orders;
            }
        }
    }
}
