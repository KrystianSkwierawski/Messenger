using Domain.Interfaces;
using Domain.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RelationShips.Command
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
                ApplicationUser invitedUser = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == request.UserName);
                ApplicationUser invitingUser = _context.ApplicationUsers.FirstOrDefault(x => x.Id == request.CurrentUserId);

                if(invitedUser != null)
                {
                    RelationShip relationShip = new RelationShip()
                    {
                        InvitedUserId = invitedUser.Id,
                        InvitingUserId = invitingUser.Id,
                        IsAccepted = false
                    };

                    _context.RelationShips.Add(relationShip);

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
