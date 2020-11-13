using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicationUsers.Commands
{
    public class UpdateThemeByUserIdCommand : IRequest
    {
        public string Theme { get; set; }
        public string UserId { get; set; } 

        public class UpdateThemeByUserIdCommandHandler : IRequestHandler<UpdateThemeByUserIdCommand>
        {
            readonly IContext _context;

            public UpdateThemeByUserIdCommandHandler(IContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateThemeByUserIdCommand request, CancellationToken cancellationToken)
            {
                ApplicationUser user = await _context.ApplicationUsers.FindAsync(request.UserId);

                if(user != null)
                {
                    user.Theme = request.Theme;
                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
