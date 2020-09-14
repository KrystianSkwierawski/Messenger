using Domain.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Friends.Command
{
    public class AddFriendCommand : IRequest
    {
        public ApplicationUser User { get; set; }
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

                if(user != null)
                {
                    Friend friend = new Friend()
                    {
                        ApplicationUser = request.User,
                        IsAccepted = false
                    };

                    user.Friends.Add(friend);

                    _context.SaveChanges();
                }

                return Unit.Value;
            }
        }

    }
}
