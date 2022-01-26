namespace Application.Common.Interfaces
{
    public interface IAudioFileBuilder
    {
        public string SaveAudio(string webRootPath, string chunks);
        public void RemoveAudio(string webRootPath, string fileNameWithExtenstion);

    }
}
