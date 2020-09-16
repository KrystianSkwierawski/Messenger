using Domain.Interfaces;
using Domain.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RelationShips.Query
{
    public class GetRelationShipsQuery : IRequest<IQueryable<RelationShip>>
    {
        public string Id { get; set; }


        public class GetRelationShipsQueryHandler : IRequestHandler<GetRelationShipsQuery, IQueryable<RelationShip>>
        {
            private readonly IContext _context;

            public GetRelationShipsQueryHandler(IContext context)
            {
                _context = context;
            }

            public async Task<IQueryable<RelationShip>> Handle(GetRelationShipsQuery request, CancellationToken cancellationToken)
            {
                IQueryable<RelationShip> relationShips = Enumerable.Empty<RelationShip>().AsQueryable();

                relationShips = _context.RelationShips.Where(x => x.InvitedUserId == request.Id || x.InvitingUserId == request.Id);

                foreach (var relationShip in relationShips)
                {
                    relationShip.InvitedUser = _context.ApplicationUsers.FirstOrDefault(x => x.Id == relationShip.InvitedUserId);
                    relationShip.InvitingUser = _context.ApplicationUsers.FirstOrDefault(x => x.Id == relationShip.InvitingUserId);
                }

                return relationShips;
            }
        }
    }
}
