using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicationUsers.Commands
{
    public class UpdateImageUrlCommand : IRequest
    {
        public string ImageUrl { get; set; }
        public string UserId { get; set; }

        public class UpdateImageUrlCommandHandler : IRequestHandler<UpdateImageUrlCommand>
        {
            readonly IContext _context;

            public UpdateImageUrlCommandHandler(IContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateImageUrlCommand request, CancellationToken cancellationToken)
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
