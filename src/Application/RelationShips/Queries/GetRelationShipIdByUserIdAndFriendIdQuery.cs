using Domain.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RelationShips.Queries
{
    public class GetRelationShipIdByUserIdAndFriendIdQuery : IRequest<int>
    {
        public string CurrentUserId { get; set; }
        public string FriendId { get; set; }

        public class GetRelationShipIdByUserIdAndFriendIdQueryHandler : IRequestHandler<GetRelationShipIdByUserIdAndFriendIdQuery, int>
        {
            readonly IContext _context;

            public GetRelationShipIdByUserIdAndFriendIdQueryHandler(IContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(GetRelationShipIdByUserIdAndFriendIdQuery request, CancellationToken cancellationToken)
            {
                 int relationShipId = _context.RelationShips.FirstOrDefault(x =>
                     (x.InvitedUserId == request.CurrentUserId || x.InvitingUserId == request.CurrentUserId) &&
                     (x.InvitedUserId == request.FriendId || x.InvitingUserId == request.FriendId)
                    ).Id;

                return relationShipId;
            }
        }
    }
}
