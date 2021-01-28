using Application.RelationShips.Queries;
using System.Threading.Tasks;
using FluentAssertions;
using Domain.Entities;
using NUnit.Framework;

namespace Messenger.Application.IntegrationTests.RelationShips.Queries
{
    using static Testing;

    public class GetRelationShipsByUserIdTests
    {
        [Test]
        public async Task ShouldReturnRelationShip()
        {
            // Arrange
            await AddAsync(new RelationShip
            {
                IsAccepted = true,
                InvitedUserId = "4e716ce2-b28a-4b9a-83b1-c5fc9486ce08",
                InvitingUserId = "90b7fb31-3edf-45e6-9c13-d9697d39b384"
            });

            var query = new GetRelationShipsByUserIdQuery()
            {
                Id = "90b7fb31-3edf-45e6-9c13-d9697d39b384"
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
