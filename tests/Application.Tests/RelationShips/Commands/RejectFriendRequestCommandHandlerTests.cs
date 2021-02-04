using Application.RelationShips.Commands;
using Application.Tests.Common;
using Domain.Entities;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.RelationShips.Commands.RejectFriendRequestCommand;

namespace Application.Tests.RelationShips.Commands
{
    public class RejectFriendRequestCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task ShouldRemoveRelationShip()
        {
            //Arrange
            ApplicationUser invitedUser = new ApplicationUser();
            ApplicationUser invitingUser = new ApplicationUser();

            await _context.ApplicationUsers.AddAsync(invitedUser);
            await _context.ApplicationUsers.AddAsync(invitingUser);

            RelationShip relationShip = new RelationShip
            {
                IsAccepted = false,
                InvitedUserId = invitedUser.Id,
                InvitingUserId = invitingUser.Id,
            };

            await _context.RelationShips.AddAsync(relationShip);

            await _context.SaveChangesAsync();

            var handler = new RejectFriendRequestCommandHandler(_context);

            //Act
            var result = await handler.Handle(new RejectFriendRequestCommand
            {
                InvitedUserId = invitedUser.Id,
                InvitingUserId = invitingUser.Id
            }, CancellationToken.None);


            //Assert
            _context.RelationShips.Find(relationShip.Id).Should().BeNull();
        }
    }
}
