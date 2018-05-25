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

        public IActionResult GetProduct(int id)
        {
            var product = db.Product.FirstOrDefault(p => p.Id == id);

            List<string> galleryImages = new List<string>();

            db.Image.Where(img => img.ProductId == product.Id).ToList()
                    .ForEach(img => 
                        galleryImages.Add(CloudinaryClient.GetImage(img.Id)));

            var productViewModel = new ProductViewModel()
            {
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                SkuCode = product.Sku_code,
                MainImage = CloudinaryClient.GetImage(product.MainImageId),
                GalleryImages = galleryImages
            };

            return View(productViewModel);
        }

        public IActionResult AddProduct(ProductViewModel model)
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
                imageIds.Add(mainImageId);

                var product = new Product()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    Category_id = model.CategoryId,
                    Discount = model.Discount,
                    Sku_code = model.SkuCode,
                    MainImageId = mainImageId
                };

                db.Product.Add(product);
                    
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
    }


}