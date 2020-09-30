using Domain.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RelationShips.Query
{
    public class GetRelationShipIdByUserIdAndFriendId : IRequest<int>
    {
        public string CurrentUserId { get; set; }
        public string FriendId { get; set; }

        public class GetRelationShipIdByUserIdAndFriendIdHandler : IRequestHandler<GetRelationShipIdByUserIdAndFriendId, int>
        {
            readonly IContext _context;

            public GetRelationShipIdByUserIdAndFriendIdHandler(IContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(GetRelationShipIdByUserIdAndFriendId request, CancellationToken cancellationToken)
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
