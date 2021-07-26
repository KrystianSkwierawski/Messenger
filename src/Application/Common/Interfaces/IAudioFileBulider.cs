namespace Application.Common.Interfaces
{
    public interface IAudioFileBulider
    {
        public string SaveAudio(string webRootPath, string chunks);
        public void RemoveAudio(string webRootPath, string fileNameWithExtenstion);

    }
}
