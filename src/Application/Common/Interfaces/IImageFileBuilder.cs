using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IImageFileBuilder
    {
        public void ConvertAndSaveImage(ImageFile imageFile);

        public void RemoveOldImage(ImageFile ImageFile);

        public string GetExtension(ImageFile ImageFile);
    }
}
