using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Data;
using FiveDevsShop.Models;
using System.Net.Http;
using System.Threading.Tasks;
using FiveDevsShop.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using FiveDevsShop.Models.DomainServices;

namespace FiveDevsShop.Controllers
{
    public class ProductController : Controller
    {
		private readonly ApplicationDbContext db;
        // Only one should be instantiated throughout the whole application
		private static readonly HttpClient client = new HttpClient();

        public ProductController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var product = db.Product.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return View("NotFound");

            var productViewModel = BuildProductViewModel(product);

            // TODO: handle not found item

            return View(productViewModel);
        }

        public IActionResult SearchProduct(string name)
        {
            if (name != null)
            {
                string[] splitName = name.Split(null);

                var products = db.Product.ToList();

                foreach (var word in splitName)
                {
                    for (int i = products.Count() - 1; i >= 0; i--)
                    {
                        if (!products.ElementAt(i).Title.ToLower().Contains(word.ToLower()))
                        {
                            products.RemoveAt(i);
                        }
                    }
                }

                return View(new ProductSearchViewModel()
                {
                    Query = name,
                    Products = Paging.LoadPage(products, 1),
                });
            }
            else
            {
                return SearchProduct("");
            }   
        }


        [HttpPost]
        public IActionResult AddProduct(AddProductViewModel model)
        {
            model.Categories = db.Category.ToList();

            if (ModelState.IsValid)
            {
                //Show error
                if(!IsImageValid(model.MainImageFile))
                    return View(model);

                foreach (var image in model.Images)
                {
                    var valid = IsImageValid(image);
                    if(!valid)
                    {
                        // Show error that only images allowed
                        return View(model);
                    }
                }

                var filePath = Path.GetTempFileName();
                var imageIds = new List<String>();

                foreach (var formFile in model.Images)
                {
                    var imageId = UploadImage(formFile, filePath);
                    imageIds.Add(imageId);
                }

                var mainImageId = UploadImage(model.MainImageFile, filePath);

                var product = new Product()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Discount = model.Discount,
                    SkuCode = model.SkuCode,
                    MainImageId = mainImageId
                };

                db.Product.Add(product);
                db.SaveChanges();
                    
                imageIds.ForEach(id => db.Image.Add(
                        new Image() { Id = id, ProductId = product.Id } ));

                db.SaveChanges();

                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult AddProductToCart(GetProductViewModel model)
        {
            var product = db.Product.FirstOrDefault(p => p.Id == model.Id);
            var productViewModel = BuildProductViewModel(product);

            // TODO cart logic

            return View("GetProduct", productViewModel);
        }

        private bool IsImageValid(IFormFile file)
        {
            int ImageMinimumBytes = 512;

            if (file.ContentType.ToLower() != "image/jpg" &&
                file.ContentType.ToLower() != "image/jpeg" &&
                file.ContentType.ToLower() != "image/pjpeg" &&
                file.ContentType.ToLower() != "image/gif" &&
                file.ContentType.ToLower() != "image/x-png" &&
                file.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".jpg" &&
                Path.GetExtension(file.FileName).ToLower() != ".png" &&
                Path.GetExtension(file.FileName).ToLower() != ".gif" &&
                Path.GetExtension(file.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            if (file.Length < ImageMinimumBytes)
            {
                return false;
            }

            return true;
        }

        private string UploadImage(IFormFile formFile, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            var imageId = Guid.NewGuid().ToString();

            CloudinaryClient.UploadImage(filePath, imageId);

            return imageId;
        }

        private GetProductViewModel BuildProductViewModel(Product product)
        {
            List<string> galleryImages = new List<string>();

            var imagesUrls = db.Image.Where(img => img.ProductId == product.Id)
                              .Select(img => CloudinaryClient.GetImageUrl(img.Id)).ToList();

            var productViewModel = new GetProductViewModel()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                SkuCode = product.SkuCode,
                Discount = product.Discount,
                MainImageUrl = CloudinaryClient.GetImageUrl(product.MainImageId),
                GalleryImagesUrls = imagesUrls
            };

            var categories = db.Category.ToList();

            var currentCategory = categories.FirstOrDefault(category => category.Id == product.CategoryId);

            while (currentCategory != null)
            {
                productViewModel.CategoryList.Insert(0, currentCategory);
                currentCategory = categories.FirstOrDefault(category => category.Id == currentCategory.Parent_id);
            }

            return productViewModel;
        }
    }


}