using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace FiveDevsShop.Services
{
    public class CloudinaryClient : IImageUploader
    {
        private static readonly Account account = new Account(
            AppSettingsProvider.CloudinaryCloud,
            AppSettingsProvider.CloudinaryApiKey,
            AppSettingsProvider.CloudinarytSecret);

        private static readonly Cloudinary cloudinary = new Cloudinary(account);

        private static string baseUrl = "http://res.cloudinary.com/five-devs-shop/image/upload/";
        private static string imgType = ".jpg";

        public void UploadImage(string filePath, string imageId)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath),
                PublicId = imageId
            };

            var uploadResult = cloudinary.Upload(uploadParams);
        }

        public string GetImageUrl(string imageId)
        {
            return baseUrl + imageId + imgType;
        }
    }
}