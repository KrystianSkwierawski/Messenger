namespace Application.Common.Interfaces
{
    public interface IAvatarPath
    {
        public string DefaultAvatarPath { get; }
        public string GetAvatarsPath(string webRootPath);
    }
}
