using System.IO;

namespace Application.Common.Models
{
    public static class AvatarPath
    {
        public static string DefaultAvatarPath { get; } = @"\images\avatars\default-avatar.png";

        public static string GetAvatarsPath(string webRootPath)
        {
            return Path.Combine(webRootPath, @"images\avatars\");
        }
    }
}
