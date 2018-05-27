using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace FiveDevsShop.Services
 {
     public static class CloudinaryClient
     {
         private static readonly Account account = new Account(
             AppSettingsProvider.CloudinaryCloud,
             AppSettingsProvider.CloudinaryApiKey,
             AppSettingsProvider.CloudinarytSecret);
             
         private static readonly Cloudinary cloudinary = new Cloudinary(account);

         private static string baseUrl= "http://res.cloudinary.com/five-devs-shop/image/upload/";
         private static string imgType = ".jpg";
         
         public static void UploadImage(string filePath, string imageId)
         {
             var uploadParams = new ImageUploadParams()
             {
                 File = new FileDescription(filePath),
                 PublicId = imageId
             };
     
             var uploadResult = cloudinary.Upload(uploadParams);
         }

        public static string GetImageUrl(string imageId)
        {
            return baseUrl + imageId + imgType;
        }
     }
     

 }