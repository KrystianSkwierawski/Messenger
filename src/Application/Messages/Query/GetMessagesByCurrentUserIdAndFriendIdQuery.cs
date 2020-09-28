using Domain.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Query
{
    public class GetMessagesByCurrentUserIdAndFriendIdQuery : IRequest<List<Message>>
    {
        public string FriendId { get; set; }
        public string CurrentUserId { get; set; }

        public class GetMessagesByCurrentUserIdAndFriendIdQueryHandler : IRequestHandler<GetMessagesByCurrentUserIdAndFriendIdQuery, List<Message>>
        {
            readonly IContext _context;

            public GetMessagesByCurrentUserIdAndFriendIdQueryHandler(IContext context)
            {
                _context = context;
            }

            public async Task<List<Message>> Handle(GetMessagesByCurrentUserIdAndFriendIdQuery request, CancellationToken cancellationToken)
            {
                List<Message> messages = messages = _context.RelationShips.Include(x => x.Messages).FirstOrDefault(x =>
                     (x.InvitedUserId == request.CurrentUserId || x.InvitingUserId == request.CurrentUserId) &&
                     (x.InvitedUserId == request.FriendId || x.InvitingUserId == request.FriendId)
                    ).Messages;

                foreach (var message in messages)
                {
                    message.ApplicationUser = _context.ApplicationUsers.FirstOrDefault(x => x.Id == message.ApplicationUserId);
                }

                return messages;
            }
        }
    }
}
