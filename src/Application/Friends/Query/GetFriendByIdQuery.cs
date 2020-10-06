using Domain.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicationUsers.Query
{
    public class GetFriendByIdQuery : IRequest<ApplicationUser>
    {
        public string Id { get; set; }

        public class GetFriendByIdQueryHandler : IRequestHandler<GetFriendByIdQuery, ApplicationUser>
        {
            readonly IContext _context;

            public GetFriendByIdQueryHandler(IContext context)
            {
                _context = context;
            }

            public async Task<ApplicationUser> Handle(GetFriendByIdQuery request, CancellationToken cancellationToken)
            {
                return await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.Id);
            }
        }
    }
}
