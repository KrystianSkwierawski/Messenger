using Application.RelationShips.Commands;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.RelationShips.Commands.AcceptFriendRequestCommand;

namespace Application.UnitTests.RelationShips.Commands
{
    public class AcceptFriendRequestCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task Should_change_relationShip_isAccepted_to_true()
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

            var handler = new AcceptFriendRequestCommandHandler(_context);

            //Act
            var result = await handler.Handle(new AcceptFriendRequestCommand
            {
                InvitedUserId = invitedUser.Id,
                InvitingUserId = invitingUser.Id
            }, CancellationToken.None);


            //Assert
            _context.RelationShips.Find(relationShip.Id).IsAccepted.Should().BeTrue();
        }
    }
}
