using Application.Common.Interfaces;
using System.IO;

namespace Infrastructure.Services
{
    public class AvatarPathService : IAvatarPath
    {
        public string DefaultAvatarPath => @"\images\avatars\default-avatar.png";

        public string GetAvatarsPath(string webRootPath)
        {
            return Path.Combine(webRootPath, @"images\avatars\");
        }
    }
}
