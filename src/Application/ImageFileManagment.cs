﻿using Microsoft.AspNetCore.Http;
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
    public class ImageFileManagment
    {
        public static string DefaultAvatarPath = @"\images\avatars\default_avatar.jpg";

        private string _avatarsPath { get; set; }
        private string _fileName { get; set; }
        private string _extenstion { get; set; }
        private IFormFile _file { get; set; }
        private string _webRootPath { get; set; }
        private string  _imageUrl { get; set; }

        public ImageFileManagment(string fileName, string extenstion, IFormFile file, string webRootPath, string imageUrl)
        {
            _avatarsPath = Path.Combine(webRootPath, @"images\avatars\");
            _fileName = fileName;
            _extenstion = extenstion.ToLower();
            _file = file;
            _webRootPath = webRootPath;
            _imageUrl = imageUrl;
        }

        private void CopyImageToWebRoot(MemoryStream Image)
        {
            using (var filesStreams = new FileStream(Path.Combine(_avatarsPath, _fileName + _extenstion), FileMode.Create))
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

        public static bool CheckIfTheImageExists(int filesCount)
        {
            return filesCount > 0 ? true : false;
        }

        public void RemoveOldImage()
        {
            string imagePath = Path.Combine(_webRootPath, _imageUrl.TrimStart('\\'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
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

