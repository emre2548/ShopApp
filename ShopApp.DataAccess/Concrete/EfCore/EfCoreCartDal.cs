using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreCartDal : EfCoreGenericRepository<Cart, ShopContext>, ICartDal
    {
        public override void Update(Cart entity)
        {
            using (var context = new ShopContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }
        public Cart getByUserId(string userId)
        {
            using (var context = new ShopContext())
            {
                return context
                    .Carts
                    .Include(i => i.CartItems)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault(i => i.UserId == userId);
            }
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            using (var context = new ShopContext())
            {
                /*
                 * @ ifadeleri SQL'de değişken oluştururken
                 * declare @Id int şeklinde değişken oluşturuyoruz
                 * set @Id=5 şeklinde değer atıyoruz
                 * print(@Id) şeklinde yazdırıyoruz burada @ ile değişkene deeğer
                 * veriyoruz buradada @ ile değişkene değer atıyoruz @ ifadesi SQL'den geliyor
                 */
                var cmd = @"delete from CartItem where CartId=@p0 and ProductId=@p1";
                context.Database.ExecuteSqlRaw(cmd, cartId, productId);
            }
        }

        public void ClearCart(string cartId)
        {
            using (var context = new ShopContext())
            {
                var cmd = @"delete from CartItem where CartId=@p0";
                context.Database.ExecuteSqlRaw(cmd, cartId);
            }
        }
    }
}
