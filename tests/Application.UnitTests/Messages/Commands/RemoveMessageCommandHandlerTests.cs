using Application.Messages.Commands;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Messages.Commands.RemoveMessageCommand;

namespace Application.UnitTests.Messages.Commands
{
    public class RemoveMessageCommandHandlerTests : CommandTestBase
    {

        [Fact]
        public async Task ShouldRemoveMessage()
        {
            //Arrange
            Message message = new Message();

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            var handler = new RemoveMessageCommandHandler(_context);

            //Act
            var result = await handler.Handle(new RemoveMessageCommand { MessageId = message.Id }, CancellationToken.None);

            //Assert
            _context.Messages.Find(message.Id).Should().BeNull();
        }
    }
}
