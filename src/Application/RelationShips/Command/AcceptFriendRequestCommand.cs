using Domain.Interfaces;
using Domain.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RelationShips.Command
{
    public class AcceptFriendRequestCommand : IRequest
    {
        public string InvitedUserId { get; set; }
        public string InvitingUserId { get; set; }

        public class AcceptFriendRequestCommandHandler : IRequestHandler<AcceptFriendRequestCommand>
        {
            private readonly IContext _context;

            public AcceptFriendRequestCommandHandler(IContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
            {
                RelationShip relationShip = _context.RelationShips.FirstOrDefault(
                    x => x.InvitedUserId == request.InvitedUserId &&
                    x.InvitingUserId == request.InvitingUserId
                );

                if (relationShip != null)
                {
                    relationShip.IsAccepted = true;
                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
