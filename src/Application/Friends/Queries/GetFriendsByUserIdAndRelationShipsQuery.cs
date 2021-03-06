﻿using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Friends.Queries
{
    public class GetFriendsByUserIdAndRelationShipsQuery : IRequest<List<ApplicationUser>>
    {
        public string Id { get; set; }
        public IQueryable<RelationShip> RelationShips { get; set; }

        public class GetFriendsByUserIdAndRelationShipsQueryHandler : IRequestHandler<GetFriendsByUserIdAndRelationShipsQuery, List<ApplicationUser>>
        {
            public async Task<List<ApplicationUser>> Handle(GetFriendsByUserIdAndRelationShipsQuery request, CancellationToken cancellationToken)
            {
                List<ApplicationUser> o_friends = new List<ApplicationUser>();

                if (request.RelationShips != null)
                {
                    foreach (var relationShip in request.RelationShips)
                    {
                        if (relationShip.InvitedUserId != request.Id)
                        {
                            o_friends.Add(relationShip.InvitedUser);
                        }
                        else if (relationShip.InvitingUserId != request.Id)
                        {
                            o_friends.Add(relationShip.InvitingUser);
                        }
                    }
                }

                return o_friends;
            }
        }
    }
}
