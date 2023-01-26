using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class OrderModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name = "Adres")]  // --> bunun adı Attribute bunu çekince ismi Adres olarak görünür
        public string Address { get; set; }
        [DisplayName("Şehir")]  // --> bunun adı Attribute bunu çekince ismi Adres olarak görünür
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvv { get; set; }
        public CartModel CartModel { get; set; }

        /*
         * Order model oluşturucak isek amacımız cartmodel oluşturmaktır bunu burada yapabiliriz yoksa
         * CartController altında CheckOut içerisinde new'lememiz gerekecek genelde bu listeleme işlemerinde
         * yapılır bire bir ya da çoka çok ilişkilerde yapıyoruz her kurucu metodda her kategoriden
         * insrance aldığımızda bunu oluşutdurk ben bunu diğer tarafta kullanabileyim. Ram'de adres tutması
         * lazım ramde adres tutmaz ise buna bir şey ekleyemeyiz (Burada CartController altında CheckOut
         * methodu içerisinde bunu kullanamayız RAM'de adres tutmaz ise önce oluşturmamız gerek böylece
         * adres ile veriyi eşleştiriyor ve kullanabiliyoruz) Biryerde listeyi .Add diyip ekleyemediğinde
         * gelmez bu şekilde yapmaz isen
         */

        //public OrderModel()
        //{
        //    CartModel = new CartModel();
        //}
    }
}
