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
                bool userExist;

                Task<ApplicationUser> invitedUserTask = _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == request.UserName);
                Task<ApplicationUser> invitingUserTask = _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.CurrentUserId);

                await Task.WhenAll(invitedUserTask, invitingUserTask);

                if (invitedUserTask.Result != null)
                {
                    RelationShip relationShip = new RelationShip()
                    {
                        InvitedUserId = invitedUserTask.Result.Id,
                        InvitingUserId = invitingUserTask.Result.Id,
                        IsAccepted = false
                    };

                    await _context.RelationShips.AddAsync(relationShip);

                    await _context.SaveChangesAsync();

                    userExist = true;
                }
                else
                {
                    userExist = false;
                }

                return userExist;
            }
        }

    }
}
