using Application.Common.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application
{
    public class AudioFileBulider : IAudioFileBulider
    {
        public async Task<string> CopyAudioToWebRoot(string webRootPath, string chunks)
        {
            string extenstion = ".ogg";
            string fileNameWithExtenstion = Guid.NewGuid() + extenstion;
            string fileNameWithPath = Path.Combine(GetAudiosPath(webRootPath), fileNameWithExtenstion);

            using (FileStream fileStream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    byte[] data = Convert.FromBase64String(chunks);
                    binaryWriter.Write(data);
                }
            }

            return fileNameWithExtenstion;
        }

        public async Task RemoveAudio(string webRootPath, string fileNameWithExtenstion)
        {
            string filePath = Path.Combine(GetAudiosPath(webRootPath), fileNameWithExtenstion);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string GetAudiosPath(string webRootPath)
        {
            return Path.Combine(webRootPath, @"audios\");
        }
    }
}
