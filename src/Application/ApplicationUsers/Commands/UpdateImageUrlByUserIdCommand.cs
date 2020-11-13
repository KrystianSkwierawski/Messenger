using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicationUsers.Commands
{
    public class UpdateImageUrlByUserIdCommand : IRequest
    {
        public string ImageUrl { get; set; }
        public string UserId { get; set; }

        public class UpdateImageUrlByUserIdCommandHandler : IRequestHandler<UpdateImageUrlByUserIdCommand>
        {
            readonly IContext _context;

            public UpdateImageUrlByUserIdCommandHandler(IContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateImageUrlByUserIdCommand request, CancellationToken cancellationToken)
            {
                ApplicationUser user = await _context.ApplicationUsers.FindAsync(request.UserId);

                if (user != null)
                {
                    user.ImageUrl = request.ImageUrl;
                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
