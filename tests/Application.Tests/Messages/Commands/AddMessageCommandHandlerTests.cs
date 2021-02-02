using Application.Messages.Commands;
using Application.Tests.Common;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Messages.Commands.AddMessageCommand;

namespace Application.Tests.Messages.Commands
{
    public class AddMessageCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task ShouldAddMessage()
        {
            // Arrange
            ApplicationUser invitedUser = new ApplicationUser();
            ApplicationUser invitingUser = new ApplicationUser();

            await _context.ApplicationUsers.AddAsync(invitedUser);
            await _context.ApplicationUsers.AddAsync(invitingUser);

            RelationShip relationShip = new RelationShip { InvitedUserId = invitedUser.Id, InvitingUserId = invitingUser.Id };

            await _context.RelationShips.AddAsync(relationShip);

            await _context.SaveChangesAsync();

            Mock<IDateTime> dateTimeMock = new Mock<IDateTime>();

            var handler = new AddMessageCommandHandler(_context, dateTimeMock.Object);

            // Act
            var result = await handler.Handle(new AddMessageCommand
            {
                MessageContent = "123",
                RelationShipId = 1,
                UserId = "1"
            }, CancellationToken.None);


            // Assert
            IQueryable<Message> messages = _context.Messages;
            messages.Should().NotBeNull();
            messages.Should().HaveCount(1);
        }
    }
}
