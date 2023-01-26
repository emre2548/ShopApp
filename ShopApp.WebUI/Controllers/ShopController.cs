using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService; // Injection işlemi deniyor

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //products/telefon?page=2
        [Route("products/{category?}")]
        public IActionResult List(string category, int page=1)
        {
            /*
             NEden burada new liyoruz proje çalışırken dışarıdan liste olluşturursak 
            List<Product> product = _productService.GetAll(); yaparsak ramda yer tutacak ta ki ram de yer kalmayıp garbıç collector
            çalışana kadar ama direk burada new lersek sadece sayfa açıkkan yer tutacak
             */

            const int pageSize = 3;

            return View(new ProductListModel()
            {
                //Products = _productService.GetAll()
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentCategory = category,
                    CurrentPage = page,
                    ItemsPerPage = pageSize
                },
                Products = _productService.GetProductsByCategory(category,page,pageSize)
            });
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails((int)id);
            if (product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailsModel()
            {
                Product=product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList()
            });
        }
    }
}
