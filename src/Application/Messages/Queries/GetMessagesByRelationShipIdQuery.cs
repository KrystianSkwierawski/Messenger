using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Queries
{
    public class GetMessagesByRelationShipIdQuery : IRequest<List<Message>>
    {
        public int RelationShipId { get; set; }

        public class GetMessagesByCurrentUserIdAndFriendIdQueryHandler : IRequestHandler<GetMessagesByRelationShipIdQuery, List<Message>>
        {
            readonly IContext _context;

            public GetMessagesByCurrentUserIdAndFriendIdQueryHandler(IContext context)
            {
                _context = context;
            }

            public async Task<List<Message>> Handle(GetMessagesByRelationShipIdQuery request, CancellationToken cancellationToken)
            {
                return await _context.Messages.Include(x => x.ApplicationUser).Where(x => x.RelationShipId == request.RelationShipId).ToListAsync();
            }
        }
    }
}
