using Domain.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages.Command
{
    public class AddMessageCommand : IRequest
    {
        public string MessageContent { get; set; }
        public int RelationShipId { get; set; }
        public string UserId { get; set; }

        public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand>
        {
            readonly IContext _context;
            IDateTime _dateTime;

            public AddMessageCommandHandler(IContext context, IDateTime dateTime)
            {
                _context = context;
                _dateTime = dateTime;
            }

            public async Task<Unit> Handle(AddMessageCommand request, CancellationToken cancellationToken)
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

                return Unit.Value;
            }
        }
    }
}
