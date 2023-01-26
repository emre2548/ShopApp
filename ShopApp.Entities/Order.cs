using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public EnumOrderState OrderState { get; set; }
        public EnumPaymentTypes PaymentTypes { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OrderNote { get; set; }

        // Ödeme API Kullanılacak
        public string PaymentId { get; set; }
        public string PaymentToken { get; set; }
        public string ConversationId { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        /*
         * listview yapılarında ekleme yapılacaksa eğer bunu yapmak zorundasın başka çözümü yok
         */

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }

    public enum EnumPaymentTypes
    {
        CreditCart=0,
        Eft=1
    }

    public enum EnumOrderState
    {
        Waiting=0,
        Unpaid=1,
        Completed=2
    }
}
