using Application.RelationShips.Queries;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Messenger.Application.IntegrationTests.RelationShips.Queries
{
    using static Testing;
    public class GetRelationShipIdByUserIdAndFriendIdTests : TestBase
    {
        [Test]
        public async Task ShouldReturnRelationShipId()
        {
            //Arrange
            ApplicationUser friendUser = await CreateUserAsync("FriendUser", "Testing1234!", new string[] { });
            ApplicationUser currentUser = await CreateUserAsync("CurrentUser", "Testing1234!", new string[] { });

            await AddAsync(new RelationShip
            {
                IsAccepted = false,
                InvitedUserId = currentUser.Id,
                InvitingUserId = friendUser.Id,
            });

            var query = new GetRelationShipIdByUserIdAndFriendIdQuery
            {
                CurrentUserId = currentUser.Id,
                FriendId = friendUser.Id
            };

            // Act
            var relationShipId = await SendAsync(query);

            // Assert     
            relationShipId.Should().NotBe(null);

            var item = await FindAsync<RelationShip>(relationShipId);
            item.Should().NotBeNull();
            item.Id.Should().Be(relationShipId);
        }
    }
}
