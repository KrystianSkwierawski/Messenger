using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IImageFileBulider
    {   
        public void ConvertAndSaveImage(ImageFile imageFile);

        public void RemoveOldImage(ImageFile ImageFile);

        public string GetExtension(ImageFile ImageFile);
    }
}
