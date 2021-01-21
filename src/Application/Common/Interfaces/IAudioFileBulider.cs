using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAudioFileBulider
    {
        public Task<string> CopyAudioToWebRoot(string webRootPath, string chunks);
        public Task RemoveAudio(string webRootPath, string fileNameWithExtenstion);

    }
}
