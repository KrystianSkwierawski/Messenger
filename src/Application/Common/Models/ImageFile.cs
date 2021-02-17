using Microsoft.AspNetCore.Http;

namespace Application.Common.Models
{
    public class ImageFile
    {
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }
        public string WebRootPath { get; set; }
        public string PreviousImageUrl { get; set; }
    }
}
