using Domain.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RelationShips.Command
{
    public class RejectFriendRequestCommand : IRequest
    {
        public string InvitedUserId { get; set; }
        public string InvitingUserId { get; set; }

        public class RejectFriendRequestCommandHandler : IRequestHandler<RejectFriendRequestCommand>
        {
            private readonly IContext _context;

            public RejectFriendRequestCommandHandler(IContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
            {
                RelationShip relationShip = await _context.RelationShips.FirstOrDefaultAsync(
                    x => x.InvitedUserId == request.InvitedUserId &&
                    x.InvitingUserId == request.InvitingUserId
                );

                if (relationShip != null)
                {
                    _context.RelationShips.Remove(relationShip);
                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
