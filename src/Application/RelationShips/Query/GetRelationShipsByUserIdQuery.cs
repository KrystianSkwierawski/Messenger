﻿using Domain.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RelationShips.Query
{
    public class GetRelationShipsByUserIdQuery : IRequest<IQueryable<RelationShip>>
    {
        public string Id { get; set; }

        public class GetRelationShipsByUserIdQueryHandler : IRequestHandler<GetRelationShipsByUserIdQuery, IQueryable<RelationShip>>
        {
            private readonly IContext _context;

            public GetRelationShipsByUserIdQueryHandler(IContext context)
            {
                _context = context;
            }

            public async Task<IQueryable<RelationShip>> Handle(GetRelationShipsByUserIdQuery request, CancellationToken cancellationToken)
            {
                IQueryable<RelationShip> relationShips = Enumerable.Empty<RelationShip>().AsQueryable();

                relationShips = _context.RelationShips.Include(x => x.InvitedUser).Include(x => x.InvitingUser).Where(x => x.InvitedUserId == request.Id || x.InvitingUserId == request.Id);

                return relationShips;
            }
        }
    }
}
