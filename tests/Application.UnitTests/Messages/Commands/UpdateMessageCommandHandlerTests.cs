using Application.Messages.Commands;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Messages.Commands.UpdateMessageCommand;

namespace Application.UnitTests.Messages.Commands
{
    public class UpdateMessageCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task ShouldUpdateMessage()
        {
            //Arrange
            Message message = new Message { Content = "MessageIsNotUpdated" };
            string expectedContent = "MessageIsUpdated";

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            var handler = new UpdateMessageCommandHandler(_context);

            //Act
            var result = await handler.Handle(new UpdateMessageCommand
            {
                MessageId = message.Id,
                Content = expectedContent
            }, CancellationToken.None);


            //Assert
            _context.Messages.Find(message.Id).Content.Should().Be(expectedContent);
        }
    }
}
