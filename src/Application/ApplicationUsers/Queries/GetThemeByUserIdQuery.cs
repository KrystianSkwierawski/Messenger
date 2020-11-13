using Domain.Interfaces;
using MediatR;
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
                return _context.ApplicationUsers.FirstOrDefault(x => x.Id == request.UserId).Theme;
            }
        }
    }
}
