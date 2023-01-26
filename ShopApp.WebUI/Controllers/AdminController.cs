using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;
using Microsoft.AspNetCore.Http;

namespace ShopApp.WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }


        public IActionResult ProductList()
        {
            return View(new ProductListModel()
            {
                Products = _productService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _categoryService.GetAll();
            return View(new ProductModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel product,int[] categoryIds, IFormFile file)
        {
            ModelState.Remove("ImageUrl");
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price
                };

                if (file != null)
                {
                    entity.ImageUrl = file.FileName;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                //return View("Index");
                _productService.Create(entity,categoryIds);
                return RedirectToAction("ProductList");
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel product, int[] categoryIds, IFormFile file)
        {
            var entity = new Product();
            if (ModelState.IsValid)
            {
                entity = _productService.GetById(product.Id);

                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = product.Name;
                entity.Description = product.Description;
                entity.Price = product.Price;

                if (file != null)
                {
                    entity.ImageUrl = file.FileName;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img", file.FileName);

                    using (var stream = new FileStream(path,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                _productService.Update(entity, categoryIds);

                return RedirectToAction("ProductList");
            }

            entity = _productService.GetByIdWithCategories((int)product.Id);
            product.SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList();
            ViewBag.Categories = _categoryService.GetAll();
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }

            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories = _categoryService.GetAll()
            });
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name
                };

                _categoryService.Create(entity);

                return RedirectToAction("CategoryList");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult EditCategory(int categoryId)
        {
            var entity = _categoryService.GetByIdWithProducts(categoryId);
            if (entity != null)
            {
                return View(new CategoryModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Products = entity.ProductCategories.Select(p => p.Product).ToList()
                });
            }

            return NotFound();
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.Id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            _categoryService.Update(entity);
            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);
            return RedirectToAction("CategoryList");
        }
    }
}
