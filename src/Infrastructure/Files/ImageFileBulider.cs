using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
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
        private string _fileName { get; set; }
        private string _extenstion { get; set; }
        private IFormFile _file { get; set; }
        private string _webRootPath { get; set; }
        private string _imageUrl { get; set; }
        private IAvatarPath _avatarPath;
       
        public ImageFileBulider(string fileName, string extenstion, IFormFile file, string webRootPath, string imageUrl)
        {
            _avatarPath = new AvatarPathService();
            _extenstion = extenstion.ToLower();
            _file = file;
            _webRootPath = webRootPath;
            _imageUrl = imageUrl;
            _fileName = fileName;
        }

        private void CopyImageToWebRoot(MemoryStream Image)
        {
            string fileNameWithPath = Path.Combine(_avatarPath.GetAvatarsPath(_webRootPath), _fileName + _extenstion);

            using (var filesStreams = new FileStream(fileNameWithPath, FileMode.Create))
            {
                Image.CopyTo(filesStreams);
            }
        }

        public void ConvertAndCopyImageToWebRoot()
        {
            var encoder = GetEncoder();
            if (encoder != null)
            {
                using (var output = new MemoryStream())
                using (Image image = Image.Load(_file.OpenReadStream()))
                {
                    int width = 256;
                    int height = 256;

                    image.Mutate(x => x.Resize(width, height));
                    image.Save(output, encoder);
                    output.Position = 0;

                    CopyImageToWebRoot(output);
                }
            }
        }

        public void RemoveOldImage()
        {
            string filePath = Path.Combine(_webRootPath, _imageUrl.TrimStart('\\'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private IImageEncoder GetEncoder()
        {
            string extension = _extenstion;
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