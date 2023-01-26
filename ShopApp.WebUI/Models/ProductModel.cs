using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Entities;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60,MinimumLength = 5,ErrorMessage = "Ürün ismi minimum 5 karakter olmalıdır.")]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [StringLength(5000,MinimumLength = 20,ErrorMessage = "Ürün açıklaması en az 20 karakter en fazla 5000 karakter olmalıdır")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Fiyat Belirtiniz.")]
        [Range(1,10000)]
        public decimal? Price { get; set; }

        public List<Category> SelectedCategories = new List<Category>();
    }
}
