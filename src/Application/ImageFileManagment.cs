using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Text.RegularExpressions;

namespace Application
{
    public static class ImageFileManagment
    {
        public static string DefaultAvatarPath = @"\images\avatars\default_avatar.jpg";

        private static void CopyImageToWebRoot(string imagePath, string fileName, string extenstion, MemoryStream image)
        {
            using (var filesStreams = new FileStream(Path.Combine(imagePath, fileName + extenstion), FileMode.Create))
            {
                image.CopyTo(filesStreams);
            }
        }

        public static void ConvertAndCopyImageToWebRoot(string imagePath, string fileName, string extenstion, IFormFile file)
        {
            var encoder = GetEncoder(extenstion);
            if (encoder != null)
            {
                using (var output = new MemoryStream())
                using (Image image = Image.Load(file.OpenReadStream()))
                {
                    int width = 256;
                    int height = 256;

                    image.Mutate(x => x.Resize(width, height));
                    image.Save(output, encoder);
                    output.Position = 0;

                    CopyImageToWebRoot(imagePath, fileName, extenstion, output);
                }
            }
        }

        public static bool CheckIfTheImageExists(int filesCount)
        {
            return filesCount > 0 ? true : false;
        }

        public static void RemoveOldImage(string webRootPath, string imageUrl)
        {
            string imagePath = Path.Combine(webRootPath, imageUrl.TrimStart('\\'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        private static IImageEncoder GetEncoder(string extension)
        {
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

