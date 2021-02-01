using Application.RelationShips.Queries;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Application.IntegrationTests.RelationShips.Queries
{
    using static Testing;
    public class GetRelationShipsByUserIdTests : TestBase
    {
        [Test]
        public async Task ShouldReturnRelationShip()
        {
            //Arrange
            ApplicationUser invitingUser = await CreateUserAsync("InvitingUser", "Testing1234!", new string[] { });
            ApplicationUser invitedUser = await CreateUserAsync("InvitedUser", "Testing1234!", new string[] { });

            //await SignInAsUserAsync(invitedUser);

            await AddAsync(new RelationShip
            {
                Id = new Random().Next(),
                IsAccepted = true,
                InvitedUserId = invitedUser.Id,
                InvitingUserId = invitingUser.Id,
            });

            var result = await SendAsync(new GetRelationShipsByUserIdQuery
            {
                Id = invitingUser.Id
            });

            // Assert
            result.Should().NotBeNull();
            result.FirstOrDefault(x => x.InvitedUserId == invitedUser.Id).Should().NotBeNull();
        }
    }
}
