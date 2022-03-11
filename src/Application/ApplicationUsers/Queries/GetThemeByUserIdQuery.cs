using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicationUsers.Queries
{
    public class GetThemeByUserIdQuery : IRequest<string>
    {
        public string UserId { get; set; }

        public class GetThemeByUserIdQueryHandler : IRequestHandler<GetThemeByUserIdQuery, string>
        {
            readonly IContext _context;

            public GetThemeByUserIdQueryHandler(IContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(GetThemeByUserIdQuery request, CancellationToken cancellationToken)
            {
                return _context.ApplicationUsers
                    .AsNoTracking()
                    .Select(x => new { x.Theme, x.Id })
                    .FirstOrDefault(x => x.Id == request.UserId).Theme;
            }
        }
    }
}
