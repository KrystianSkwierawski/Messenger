using Domain.Interfaces;
using Domain.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Command
{
    public class AddMessageCommand : IRequest<Message>
    {
        public string MessageContent { get; set; }
        public int RelationShipId { get; set; }
        public string UserId { get; set; }

        public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Message>
        {
            readonly IContext _context;
            IDateTime _dateTime;

            public AddMessageCommandHandler(IContext context, IDateTime dateTime)
            {
                _context = context;
                _dateTime = dateTime;
            }

            public async Task<Message> Handle(AddMessageCommand request, CancellationToken cancellationToken)
            {
                Message message = new Message
                {
                    ApplicationUserId = request.UserId,
                    Content = request.MessageContent,
                    RelationShipId = request.RelationShipId,
                    DateSended = _dateTime.Now
                };

                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();


                message.ApplicationUser = _context.ApplicationUsers.FirstOrDefault(x => x.Id == message.ApplicationUserId);
                return message;
            }
        }
    }
}
