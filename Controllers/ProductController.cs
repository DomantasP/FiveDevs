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
using System.Diagnostics;
using OfficeOpenXml;

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

        [HttpPost]
        public IActionResult UploadProductByExcel(IFormFile file)
        {
            if (file == null) return null;
            if (!IsExcelFile(file)) return null;

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;

                Debug.WriteLine(rowCount + " " + ColCount);

                var rawText = string.Empty;
                for (int row = 2; row <= rowCount; row++)
                {
                    ProductsImportModel product = new ProductsImportModel();
                    product.Title = worksheet.Cells[row, 1].Text.Trim();
                    product.ShortDescription = worksheet.Cells[row, 2].Text.Trim();

                    Decimal price;
                    if (!Decimal.TryParse(worksheet.Cells[row, 3].Text.Trim(), out price)) return null;
                    product.Price = price;
                    
                    product.Images = worksheet.Cells[row, 4].Text.Trim().Split(' ').ToList();
                    product.SkuCode = worksheet.Cells[row, 5].Text.Trim();
                    product.Description = worksheet.Cells[row, 6].Text.Trim();

                    product.Categories = worksheet.Cells[row, 7].Text.Trim().Split('/').ToList();
                    if (product.Categories.Count > 3) return null;
                    /*for (int col = 1; col <= ColCount; col++)
                    {
                        if(worksheet.Cells[row, col].Value != null) Debug.WriteLine(worksheet.Cells[row, col].Text);//rawText += worksheet.Cells[row, col].Value.ToString() + "\t";
                    }*/
                    //rawText += "\r\n";
                }
                //_logger.LogInformation(rawText);
                //Debug.WriteLine(rawText);
            }


            return Json(filePath);
        }

        private Boolean IsExcelFile(IFormFile file)
        {
            if (file.ContentType.ToLower() != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return false;
            }
            return true;
        }

        public IActionResult GetProduct(int id)
        {
            var product = db.Product.FirstOrDefault(p => p.Id == id);

            return View(product);
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

                return View(products);
            }
            else
            {
                return View();
            }   
        }

        public IActionResult AddProduct(ProductViewModel model)
        {
            model.Categories = db.Category.ToList();

            if (ModelState.IsValid)
            {
                if (!IsImageListValid(model.Images))
                {
                    // Show error that only images allowed
                    return View(model);
                }

                var filePath = Path.GetTempFileName();
                var imageIds = new List<String>();

                var product = new Product()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Discount = model.Discount,
                    SkuCode = model.SkuCode
                };

                foreach (var formFile in model.Images)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }

                    var imageId = Guid.NewGuid().ToString();

                    try
                    {
                        CloudinaryClient.UploadImage(filePath, imageId);
                    }
                    catch (Exception)
                    {
                        // Return message that file upload failed
                        return View(model);
                    }

                    imageIds.Add(imageId);
                }

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

        private bool IsImageListValid(List<IFormFile> files)
        {

            foreach (var file in files)
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
            }

            return true;
        }
    }


}