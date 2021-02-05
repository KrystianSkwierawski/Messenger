using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicationUsers.Commands
{
    public class UpdateImageByUserIdCommand : IRequest
    {
        public string UserId { get; set; }
        public ImageFile ImageFile { get; set; }

        public class UpdateImageByUserIdCommandHandler : IRequestHandler<UpdateImageByUserIdCommand>
        {
            readonly IContext _context;
            private IImageFileBulider _imageFileBulider;

            public UpdateImageByUserIdCommandHandler(IContext context, IImageFileBulider imageFileBulider)
            {
                _context = context;
                _imageFileBulider = imageFileBulider;
            }

            public async Task<Unit> Handle(UpdateImageByUserIdCommand request, CancellationToken cancellationToken)
            {
                ApplicationUser user = await _context.ApplicationUsers.FindAsync(request.UserId);

                if (user != null)
                {
                    string defaultAvatarPath = @"\images\avatars\default-avatar.png";
                    if (user.ImageUrl != null && user.ImageUrl != defaultAvatarPath)
                    {
                        _imageFileBulider.RemoveOldImage(request.ImageFile);
                    }

                    _imageFileBulider.ConvertAndCopyImageToWebRoot(request.ImageFile);

                    user.ImageUrl = @"\images\avatars\" + request.ImageFile.FileName + request.ImageFile.Extenstion;
                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
