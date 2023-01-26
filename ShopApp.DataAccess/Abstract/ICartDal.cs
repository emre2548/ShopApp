using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICartDal : IRepository<Cart>
    {
        Cart getByUserId(string userId);
        void DeleteFromCart(int cartId, int productId);
        void ClearCart(string cartId);
    }
}
