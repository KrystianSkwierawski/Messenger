using Application.Messages.Commands;
using Application.UnitTests.Common;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Messages.Commands.AddMessageCommand;

namespace Application.UnitTests.Messages.Commands
{
    public class AddMessageCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task ShouldAddAndReturnMessage()
        {
            // Arrange
            Mock<IDateTime> dateTimeMock = new Mock<IDateTime>();

            var handler = new AddMessageCommandHandler(_context, dateTimeMock.Object);

            // Act
            Message message = await handler.Handle(new AddMessageCommand(), CancellationToken.None);

            // Assert
            message.Should().NotBeNull();

            IQueryable<Message> messages = _context.Messages;
            messages.Should().NotBeNull();
            messages.Should().HaveCount(1);
        }
    }
}
