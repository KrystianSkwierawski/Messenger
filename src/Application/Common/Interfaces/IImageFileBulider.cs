namespace Application.Common.Interfaces
{
    public interface IImageFileBulider
    {   
        public void ConvertAndCopyImageToWebRoot();

        public void RemoveOldImage();

    }
}
