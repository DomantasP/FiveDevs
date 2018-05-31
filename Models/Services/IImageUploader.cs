using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiveDevsShop.Services
{
    public interface IImageUploader
    {
        void UploadImage(string path, string id);
        string GetImageUrl(string id);
    }
}
