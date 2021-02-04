using Application.RelationShips.Commands;
using Application.Tests.Common;
using Domain.Entities;
using FluentAssertions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.RelationShips.Commands.AddRelationShipCommand;

namespace Application.Tests.RelationShips.Commands
{
    public class AddRelationShipCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task Should_AddRelationShipAndReturnTrue_IfInvitedUserExist()
        {
            //Arrange
            ApplicationUser currentUser = new ApplicationUser();
            ApplicationUser invitedUser = new ApplicationUser { UserName = "InvitedUser"};

            await _context.ApplicationUsers.AddAsync(currentUser);
            await _context.ApplicationUsers.AddAsync(invitedUser);

            await _context.SaveChangesAsync();

            var handler = new AddRelationShipCommandHandler(_context);

            //Act
            var result = await handler.Handle(new AddRelationShipCommand
            {
                CurrentUserId = currentUser.Id,
                UserName = invitedUser.UserName
            }, CancellationToken.None);

            //Assert
            result.Should().BeTrue();

            _context.RelationShips
                .FirstOrDefault(x => x.InvitedUserId == invitedUser.Id && x.InvitingUserId == currentUser.Id)
                .Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnFalse_IfInvitedUserDoesNotExist()
        {
            //Arrange
            ApplicationUser currentUser = new ApplicationUser();
            ApplicationUser invitedUser = new ApplicationUser { UserName = "InvitedUser" };

            await _context.ApplicationUsers.AddAsync(currentUser);

            await _context.SaveChangesAsync();

            var handler = new AddRelationShipCommandHandler(_context);

            //Act
            var result = await handler.Handle(new AddRelationShipCommand
            {
                CurrentUserId = currentUser.Id,
                UserName = invitedUser.UserName
            }, CancellationToken.None);

            //Assert
            result.Should().BeFalse();

            _context.RelationShips
                .FirstOrDefault(x => x.InvitedUserId == invitedUser.Id && x.InvitingUserId == currentUser.Id)
                .Should().BeNull();
        }
    }
}
