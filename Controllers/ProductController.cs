using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FiveDevsShop.Data;
using FiveDevsShop.Models;
using FiveDevsShop.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using FiveDevsShop.Models.DomainServices;
using FiveDevsShop.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Internal;

namespace FiveDevsShop.Controllers
{
    public class ProductController : Controller
    {
		private readonly ApplicationDbContext db;
        private readonly PriceCalculator priceCalculator;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IImageUploader uploader;
        private readonly Paging paging;

        public ProductController(ApplicationDbContext db, PriceCalculator priceCalculator, UserManager<ApplicationUser> userManager, IImageUploader uploader, Paging paging)
        {
            this.db = db;
            this.priceCalculator = priceCalculator;
            this.userManager = userManager;
            this.uploader = uploader;
            this.paging = paging;
        }

        [HttpPost]
        public JsonResult UploadProductByExcel(IFormFile file)
        {
            if (file == null) return null;
            if (!IsExcelFile(file)) return null;

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            FileInfo fileInfo = new FileInfo(filePath);
            List<ProductsImportModel> importProducts = new List<ProductsImportModel>();

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;
                
                
                for (int row = 2; row <= rowCount; row++)
                {
                    if (IsPropertyLine(worksheet, row))
                    {
                        importProducts[importProducts.Count - 1].PropertiesKey.Add(worksheet.Cells[row, 8].Text.Trim());
                        importProducts[importProducts.Count - 1].PropertiesValue.Add(worksheet.Cells[row, 9].Text.Trim());
                    }
                    else if (IsCorrectLine(worksheet, row))
                    {
                        ProductsImportModel product = new ProductsImportModel();
                        product.Title = worksheet.Cells[row, 1].Text.Trim();
                        product.ShortDescription = worksheet.Cells[row, 2].Text.Trim();

                        Decimal price;
                        if (!Regex.IsMatch(worksheet.Cells[row, 3].Text.Trim(), @"^[0-9]{1,8}[,.][0-9]{2}$")) return null;

                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                        if (!Decimal.TryParse(worksheet.Cells[row, 3].Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out price)) return null;
                        product.Price = price;

                        product.Images = worksheet.Cells[row, 4].Text.Trim().Split(' ').ToList();

                        product.Images = DeleteEmpty(product.Images);
                        product.SkuCode = worksheet.Cells[row, 5].Text.Trim();
                        product.Description = worksheet.Cells[row, 6].Text.Trim();

                        product.Categories = worksheet.Cells[row, 7].Text.Trim().Split('/').ToList();
                        if (product.Categories.Count > 3) return null;

                        if (worksheet.Cells[row, 8].Text.Trim() != "" && worksheet.Cells[row, 9].Text.Trim() != "")
                        {
                            product.PropertiesKey.Add(worksheet.Cells[row, 8].Text.Trim());
                            product.PropertiesValue.Add(worksheet.Cells[row, 9].Text.Trim());
                        }
                        if(!IsProductValid(product)) return null;
                        if (db.Product.FirstOrDefault(p => p.SkuCode == product.SkuCode) != null) return null;
                        importProducts.Add(product);
                    }
                    else return null;
                }        
            }
            AddProducts(importProducts);

            return Json(1);
        }

        private Boolean AddProducts(List<ProductsImportModel> excelProducts)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                Category parentCategory = new Category();
                for (int i = 0; i < excelProducts.Count; i++)
                {
                    ProductsImportModel excelProduct = excelProducts[i];
                    Category category = new Category();
                    try
                    { 
                        for (int j = 0; j < excelProduct.Categories.Count; j++)
                        {
                            if(j > 0) category = db.Category.FirstOrDefault(c => c.Title == excelProduct.Categories[j] && 
                                    c.Parent_id == parentCategory.Id);
                            else category = db.Category.FirstOrDefault(c => c.Title == excelProduct.Categories[j]);

                            if (category == null && j == 0) /*Root category*/
                            {
                                category = new Category();
                                category.Title = excelProduct.Categories[j];
                                db.Category.Add(category);
                                db.SaveChanges();
                            }
                            else if (category == null)
                            {
                                category = new Category();
                                category.Title = excelProduct.Categories[j];
                                category.Parent_id = parentCategory.Id;
                                db.Category.Add(category);
                                db.SaveChanges();
                            }
                            parentCategory = category;
                            
                        }
                    }
                    catch (Exception) { transaction.Rollback(); }
                    Product product = new Product();
                    product.SkuCode = excelProduct.SkuCode;
                    product.Price = excelProduct.Price;
                    product.Title = excelProduct.Title;
                    product.Discount = excelProduct.Discount;
                    product.Description = excelProduct.Description;
                    product.ShortDescription = excelProduct.ShortDescription;
                    product.CategoryId = category.Id;//db.Category.FirstOrDefault(c=>c.Title == excelProduct.Categories[excelProduct.Categories.Count-1]).Id;

                    try
                    {
                        db.Product.Add(product);
                        db.SaveChanges();
                    }
                    catch (Exception) { transaction.Rollback(); }

                    try
                    {
                        for (int j = 0; j < excelProduct.Images.Count; j++)
                        {
                            Image image = new Image();
                            image.Id = Guid.NewGuid().ToString();
                            image.Url = excelProduct.Images[j];
                            image.ProductId = product.Id;
                            db.Image.Add(image);
                            if (j == 0) product.MainImageId = image.Id;
                        }
                        db.SaveChanges();
                    }
                    catch (Exception) { transaction.Rollback(); }

                    try
                    {
                        for (int j = 0; j < excelProduct.PropertiesKey.Count; j++)
                        {
                            ProductProperty property = new ProductProperty();
                            property.Name = excelProduct.PropertiesKey[j];
                            property.Value = excelProduct.PropertiesValue[j];
                            property.ProductId = product.Id;
                            db.ProductProperty.Add(property);
                        }
                        db.SaveChanges();
                    }
                    catch (Exception) { transaction.Rollback(); }
                }
                transaction.Commit();
            }
            return true;
        }
        
        private List<String> DeleteEmpty(List<String> texts)
        {
            for(int i = 0; i < texts.Count; i++)
            {
                if (texts[i] == "") texts.RemoveAt(i);
            }
            return texts;
        }

        private Boolean IsProductValid(ProductsImportModel product)
        {
            if (!Regex.IsMatch(product.Title, @"^.{1,45}$")) return false;
            if (!Regex.IsMatch(product.ShortDescription, @"^.{1,400}$")) return false;

            if (product.Price <= 0) return false;

            for (int i = 0; i < product.Images.Count; i++)
            {
                if (!Regex.IsMatch(product.Images[i], @"(?:([^:/?#]+):)?(?://([^/?#]*))?([^?#]*\.(?:jpg|gif|png|jpe|jpeg|pjpeg|x-png))(?:\?([^#]*))?(?:#(.*))?")) return false;
            }

            if (!Regex.IsMatch(product.SkuCode, @"^.{1,20}$")) return false;
            if (!Regex.IsMatch(product.Description, @"^.{1,16383}$")) return false;

            for (int i = 0; i < product.Categories.Count; i++)
            {
                if (!Regex.IsMatch(product.Categories[i], @"^.{1,45}$")) return false;
            }

            for (int i = 0; i < product.PropertiesKey.Count; i++)
            {
                if (!Regex.IsMatch(product.PropertiesKey[i], @"^.{1,40}$")) return false;
                if (!Regex.IsMatch(product.PropertiesKey[i], @"^.{1,40}$")) return false;
                if (product.PropertiesKey.Count(a => a == product.PropertiesKey[i]) > 1) return false;
            }

            return true;
        }
        private Boolean IsCorrectLine(ExcelWorksheet worksheet, int row)
        {
            if (!(worksheet.Cells[row, 1].Text.Trim() != "" && worksheet.Cells[row, 2].Text.Trim() != "" &&
                worksheet.Cells[row, 3].Text.Trim() != "" && worksheet.Cells[row, 5].Text.Trim() != "" &&
                worksheet.Cells[row, 6].Text.Trim() != "" && worksheet.Cells[row, 7].Text.Trim() != ""))
            {
                return false;
            }
            else return true;
        }
        private Boolean IsPropertyLine(ExcelWorksheet worksheet, int row)
        {
            if (row == 1) return false;
            if(worksheet.Cells[row, 1].Text.Trim() == "" && worksheet.Cells[row, 2].Text.Trim() == "" &&
                worksheet.Cells[row, 3].Text.Trim() == "" && worksheet.Cells[row, 4].Text.Trim() == "" &&
                worksheet.Cells[row, 5].Text.Trim() == "" && worksheet.Cells[row, 6].Text.Trim() == "" &&
                worksheet.Cells[row, 7].Text.Trim() == "" && worksheet.Cells[row, 8].Text.Trim() != "" &&
                worksheet.Cells[row, 9].Text.Trim() != "")
            {
                return true;
            }
            return false;
        }

        private Boolean IsExcelFile(IFormFile file)
        {
            if (file.ContentType.ToLower() != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return false;
            }
            return true;
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

        public IActionResult SearchProduct(string name, int page = 1)
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

                var productView = paging.LoadPage(products, page);
                productView.AddQueryParam("name", name);
                return View(new ProductSearchViewModel()
                {
                    Query = name,
                    Products = productView,
                });
            }
            else
            {
                return SearchProduct("");
            }   
        }

        public async Task<IActionResult> ViewCart()
        {
            var user = await userManager.GetUserAsync(User);
            var cart = this.UserShoppingCart();
            return View("/Views/User/Cart.cshtml", new CartViewModel()
            {
                Cart = cart,
                LoggedIn = user != null,
            });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddProductView()
        {
            var model = new AddProductViewModel {Categories = db.Category.ToList()};

            return View(model);
        }
        

        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct(AddProductViewModel model)
        {
            model.Categories = db.Category.ToList();

            if (ModelState.IsValid)
            {
                //Show error
                if(model.MainImageFile != null && !IsImageValid(model.MainImageFile))
                    return View("AddProductView", model);

                if (model.Images != null)
                {
                    foreach (var image in model.Images)
                    {
                        var valid = IsImageValid(image);
                        if(!valid)
                        {
                            // Show error that only images allowed
                            return View("AddProductView", model);
                        }
                    }                    
                }


                var filePath = Path.GetTempFileName();
                var imageIds = new List<String>();

                if (model.Images != null)
                {
                    foreach (var formFile in model.Images)
                    {
                        var imageId = UploadImage(formFile, filePath);
                        imageIds.Add(imageId);
                    }                    
                }

                string mainImageId = "";
                
                if(model.MainImageFile != null)
                    mainImageId = UploadImage(model.MainImageFile, filePath);

                var product = new Product()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Discount = model.Discount,
                    SkuCode = model.SkuCode,
                    MainImageId = mainImageId,
                    ShortDescription = model.ShortDescription
                };

                db.Product.Add(product);
                imageIds.ForEach(id => db.Image.Add(
                        new Image() { Id = id, ProductId = product.Id } ));

                db.SaveChanges();

                return View("AddProductView", model);
            }
            else
            {
                return View("AddProductView", model);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditProductView(int id)
        {
            var product = db.Product.FirstOrDefault(p => p.Id == id);
            
            var model = new AddProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                Description =  product.Description,
                ShortDescription =  product.ShortDescription,
                Discount = product.Discount,
                SkuCode = product.SkuCode,
                Categories = db.Category.ToList(),
                CategoryId = product.CategoryId,
            };

            return View(model);
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct(AddProductViewModel model)
        {
            model.Categories = db.Category.ToList();

            if (ModelState.IsValid)
            {
                //Show error
                if(model.MainImageFile != null && !IsImageValid(model.MainImageFile))
                    return View("EditProductView", model);

                if (model.Images != null)
                {
                    foreach (var image in model.Images)
                    {
                        var valid = IsImageValid(image);
                        if(!valid)
                        {
                            // Show error that only images allowed
                            return View("EditProductView", model);
                        }
                    }                    
                }


                var filePath = Path.GetTempFileName();
                var imageIds = new List<String>();

                if (model.Images != null)
                {
                    foreach (var formFile in model.Images)
                    {
                        var imageId = UploadImage(formFile, filePath);
                        imageIds.Add(imageId);
                    }                    
                }

                string mainImageId = "";
                
                if(model.MainImageFile != null)
                    mainImageId = UploadImage(model.MainImageFile, filePath);
                
                var product = db.Product.Find(model.Id);
                product.Title = model.Title;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                product.Discount = model.Discount;
                product.SkuCode = model.SkuCode;
                product.MainImageId = mainImageId;
                product.ShortDescription = model.ShortDescription;
                
                imageIds.ForEach(id => db.Image.Add(
                        new Image() { Id = id, ProductId = product.Id } ));

                db.SaveChangesAsync();

                return View("EditProductView", model);
            }
            else
            {
                return View("EditProductView", model);
            }
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            var product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChangesAsync();

            return RedirectToAction("Product", "Admin");
        }
        
        [HttpPost]
        public IActionResult AddProductToCart(GetProductViewModel model)
        {
            var product = db.Product.FirstOrDefault(p => p.Id == model.Id);
            if (product == null)
                return View("NotFound");

            var productViewModel = BuildProductViewModel(product);
            var category = db.Category.First(c => c.Id == product.CategoryId);

            var cart = this.UserShoppingCart();
            cart.AddProduct(product, model.ProductCount, category.Title, uploader, priceCalculator);
            this.SaveCart(cart);

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

            uploader.UploadImage(filePath, imageId);

            return imageId;
        }

        private GetProductViewModel BuildProductViewModel(Product product)
        {
            List<string> galleryImages = new List<string>();

            var imagesUrls = db.Image.Where(img => img.ProductId == product.Id)
                              .Select(img => uploader.GetImageUrl(img.Id)).ToList();

            var productViewModel = new GetProductViewModel()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                SkuCode = product.SkuCode,
                Discount = product.Discount,
                MainImageUrl = uploader.GetImageUrl(product.MainImageId),
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