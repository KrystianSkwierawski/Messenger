using Application.Common.Interfaces;
using Application.Common.Models;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Files
{
    public class ImageFileBulider : IImageFileBulider
    {
        private void SaveImage(ImageFile imageFile, Image image)
        {
            string avatarsPath = Path.Combine(imageFile.WebRootPath, @"images\avatars\");
            string fileNameWithPath = Path.Combine(avatarsPath, imageFile.FileName + GetExtension(imageFile));

            image.Save(fileNameWithPath);
        }

        public void ConvertAndSaveImage(ImageFile imageFile)
        {
            using var image = Image.Load(imageFile.FormFile.OpenReadStream());

            int width = 256;
            int height = 256;

            image.Mutate(x => x.Resize(width, height));

            SaveImage(imageFile, image);
        }

        public void RemoveOldImage(ImageFile ImageFile)
        {
            string filePath = Path.Combine(ImageFile.WebRootPath, ImageFile.PreviousImageUrl.TrimStart('\\'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public string GetExtension(ImageFile imageFile)
        {
            return Path.GetExtension(imageFile.FormFile.FileName).ToLower();
        }
    }
}