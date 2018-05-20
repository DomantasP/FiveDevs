using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace FiveDevsShop.Services
 {
     public class CloudinaryClient
     {
         // Put these into the config file
         private static readonly Account account = new Account(
             "five-devs-shop",
             "396914585999764",
             "wE1cISeIFct9Ki5grzal_xampY4");
             
         private static readonly Cloudinary cloudinary = new Cloudinary(account);
         
         public static void UploadImage(string filePath)
         {
             var uploadParams = new ImageUploadParams()
             {
                 File = new FileDescription(@"./img.jpg")
             };
     
             var uploadResult = cloudinary.Upload(uploadParams);
         }
     }
     

 }