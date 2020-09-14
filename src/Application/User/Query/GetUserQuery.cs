using Domain.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Query
{
    public class GetUserQuery : IRequest<ApplicationUser>
    {
        public string Id { get; set; }


        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ApplicationUser>
        {
            private readonly IContext _context;

            public GetUserQueryHandler(IContext context)
            {
                _context = context;
            }

            public async Task<ApplicationUser> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                ApplicationUser user = new ApplicationUser();

                user = _context.ApplicationUsers.Include(x => x.Friends).FirstOrDefault(x => x.Id == request.Id);

                return user;
            }
        }
    }
}
