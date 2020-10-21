using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RelationShips.Commands
{
    public class AddRelationShipCommand : IRequest<bool>
    {
        public string CurrentUserId { get; set; }
        public string UserName { get; set; }

        public class AddRelationShipCommandHandler : IRequestHandler<AddRelationShipCommand, bool>
        {
            private readonly IContext _context;

            public AddRelationShipCommandHandler(IContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(AddRelationShipCommand request, CancellationToken cancellationToken)
            {
                ApplicationUser invitedUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == request.UserName);
                ApplicationUser invitingUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.CurrentUserId);

                if (invitedUser != null)
                {
                    RelationShip relationShip = new RelationShip()
                    {
                        InvitedUserId = invitedUser.Id,
                        InvitingUserId = invitingUser.Id,
                        IsAccepted = false
                    };

                    await _context.RelationShips.AddAsync(relationShip);

                    await _context.SaveChangesAsync();

                    return true; //user exist
                }
                else
                {
                    return false; //user doesn't exist
                }
            }
        }

    }
}
