using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Commands
{
    public class UpdateMessageCommand : IRequest
    {
        public int MessageId { get; set; }
        public string Content { get; set; }

        public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand>
        {
            readonly IContext _context;

            public UpdateMessageCommandHandler(IContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
            {
                Message message = await _context.Messages.FindAsync(request.MessageId);

                if(message != null)
                {
                    message.Content = request.Content;

                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
