using Application.Common.Interfaces;
using Application.Common.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Text.RegularExpressions;

namespace Infrastructure.Files
{
    public class ImageFileBulider : IImageFileBulider
    {
        private void CopyImageToWebRoot(ImageFile imageFile)
        {
            string avatarsPath = Path.Combine(imageFile.WebRootPath, @"images\avatars\");
            string fileNameWithPath = Path.Combine(avatarsPath, imageFile.FileName + imageFile.Extenstion);

            using (var filesStreams = new FileStream(fileNameWithPath, FileMode.Create))
            {
                imageFile.FormFile.CopyTo(filesStreams);
            }
        }

        public void ConvertAndCopyImageToWebRoot(ImageFile imageFile)
        {
            var encoder = GetEncoder(imageFile.Extenstion);
            if (encoder != null)
            {
                using (var output = new MemoryStream())
                using (Image image = Image.Load(imageFile.FormFile.OpenReadStream()))
                {
                    int width = 256;
                    int height = 256;

                    image.Mutate(x => x.Resize(width, height));
                    image.Save(output, encoder);
                    output.Position = 0;

                    CopyImageToWebRoot(imageFile);
                }
            }
        }

        public void RemoveOldImage(ImageFile ImageFile)
        {
            string filePath = Path.Combine(ImageFile.WebRootPath, ImageFile.ImageUrl.TrimStart('\\'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private IImageEncoder GetEncoder(string extenstion)
        {
            string extension = extenstion;
            IImageEncoder encoder = null;

            extension = extension.Replace(".", "");

            var isSupported = Regex.IsMatch(extension, "gif|png|jpe?g", RegexOptions.IgnoreCase);

            if (isSupported)
            {
                switch (extension)
                {
                    case "png":
                        encoder = new PngEncoder();
                        break;
                    case "jpg":
                        encoder = new JpegEncoder();
                        break;
                    case "jpeg":
                        encoder = new JpegEncoder();
                        break;
                    case "gif":
                        encoder = new GifEncoder();
                        break;
                    default:
                        break;
                }
            }

            return encoder;
        }
    }
}