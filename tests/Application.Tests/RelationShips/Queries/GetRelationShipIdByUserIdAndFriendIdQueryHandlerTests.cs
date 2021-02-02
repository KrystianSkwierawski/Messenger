using Application.IntegrationTests.Common;
using Application.RelationShips.Queries;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.RelationShips.Queries.GetRelationShipIdByUserIdAndFriendIdQuery;

namespace Application.Tests.RelationShips.Queries
{
    [Collection("QueryCollection")]
    public class GetRelationShipIdByUserIdAndFriendIdQueryHandlerTests
    {
        private readonly Context _context;
        public GetRelationShipIdByUserIdAndFriendIdQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task ShouldReturnRelationShip()
        {
            //Arrange
            ApplicationUser currentUser = new ApplicationUser();
            ApplicationUser friendUser = new ApplicationUser();

            await _context.ApplicationUsers.AddAsync(currentUser);
            await _context.ApplicationUsers.AddAsync(friendUser);

            RelationShip relationShip = new RelationShip
            {
                IsAccepted = true,
                InvitedUserId = currentUser.Id,
                InvitingUserId = friendUser.Id,
            };

            await _context.RelationShips.AddAsync(relationShip);

            await _context.SaveChangesAsync();

            var handler = new GetRelationShipIdByUserIdAndFriendIdQueryHandler(_context);

            //Act
            int relationShipId = await handler.Handle(new GetRelationShipIdByUserIdAndFriendIdQuery
            {
                CurrentUserId = currentUser.Id,
                FriendId = friendUser.Id
            }, CancellationToken.None);

            //Assert
            relationShipId.GetType().Should().Be(typeof(int));
            relationShipId.Should().Be(relationShip.Id);
        }
    }
}
