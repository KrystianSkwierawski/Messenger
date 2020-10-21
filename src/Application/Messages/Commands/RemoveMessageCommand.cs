using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Commands
{
    public class RemoveMessageCommand : IRequest
    {
        public int MessageId { get; set; }

        public class RemoveMessageCommandHandler : IRequestHandler<RemoveMessageCommand>
        {
            readonly IContext _context;

            public RemoveMessageCommandHandler(IContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(RemoveMessageCommand request, CancellationToken cancellationToken)
            {
                Message message = await _context.Messages.FindAsync(request.MessageId);

                if(message != null)
                {
                    _context.Messages.Remove(message);
                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
