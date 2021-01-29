using Application.IntegrationTests.Common;
using Application.RelationShips.Queries;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.IntegrationTests.RelationShips.Queries
{
    [Collection("QueryCollection")]
    public class GetRelationShipsByUserIdQueryHandlerTests
    {
        private readonly Context _context;
        public GetRelationShipsByUserIdQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task ShouldReturnRelationShips()
        {
            //Arrange
            ApplicationUser invitedUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "invitedUser"
            };

            ApplicationUser invitingUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "invitingUser"
            };

            await _context.ApplicationUsers.AddAsync(invitedUser);
            await _context.ApplicationUsers.AddAsync(invitingUser);

            RelationShip relationShip = new RelationShip
            {
                IsAccepted = true,
                InvitedUserId = invitedUser.Id,
                InvitingUserId = invitingUser.Id,
            };

            await _context.AddAsync(relationShip);

            await _context.SaveChangesAsync();

            var handler = new GetRelationShipsByUserIdQuery.GetRelationShipsByUserIdQueryHandler(_context);

            //Act 
            var result = await handler.Handle(new GetRelationShipsByUserIdQuery
            {
                Id = invitedUser.Id

            }, CancellationToken.None);


            //Assert
            result.Should().NotBeNull();
            result.First().InvitedUserId.Should().Be(invitedUser.Id);
            result.First().InvitingUserId.Should().Be(invitingUser.Id);
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task ShouldReturnEmpty()
        {
            //Arrange
            ApplicationUser user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "User1"
            };

            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            var handler = new GetRelationShipsByUserIdQuery.GetRelationShipsByUserIdQueryHandler(_context);

            //Act 
            var result = await handler.Handle(new GetRelationShipsByUserIdQuery
            {
                Id = user.Id

            }, CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }
    }
}
