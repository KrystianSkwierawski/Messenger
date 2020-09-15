using Domain.Interfaces;
using Domain.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Friends.Command
{
    public class AddFriendCommand : IRequest
    {
        public string CurrentUserId { get; set; }
        public string UserName { get; set; }

        public class AddFriendCommandHandler : IRequestHandler<AddFriendCommand>
        {
            private readonly IContext _context;

            public AddFriendCommandHandler(IContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AddFriendCommand request, CancellationToken cancellationToken)
            {
                ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == request.UserName);
                ApplicationUser currentUser = _context.ApplicationUsers.FirstOrDefault(x => x.Id == request.CurrentUserId);

                if(user != null)
                {
                    RelationShip relationShip = new RelationShip()
                    {
                        InvitedUserId = user.Id,
                        InvitingUserId = currentUser.Id,
                        IsAccepted = false
                    };

                    _context.RelationShips.Add(relationShip);

                    _context.SaveChanges();
                }

                return Unit.Value;
            }
        }

    }
}
