using Application.IntegrationTests.Common;
using Application.RelationShips.Queries;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
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
                UserName = "invitedUser"
            };

            ApplicationUser invitingUser = new ApplicationUser
            {
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

            await _context.RelationShips.AddAsync(relationShip);

            await _context.SaveChangesAsync();

            var handler = new GetRelationShipsByUserIdQuery.GetRelationShipsByUserIdQueryHandler(_context);

            //Act 
            IQueryable<RelationShip> relationShips = await handler.Handle(new GetRelationShipsByUserIdQuery
            {
                Id = invitedUser.Id

            }, CancellationToken.None);


            //Assert
            relationShips.Should().NotBeNull();
            relationShips.First().InvitedUserId.Should().Be(invitedUser.Id);
            relationShips.First().InvitingUserId.Should().Be(invitingUser.Id);
            relationShips.Should().HaveCount(1);
        }

        [Fact]
        public async Task ShouldReturnEmpty()
        {
            //Arrange
            ApplicationUser user = new ApplicationUser
            {
                UserName = "User1"
            };

            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            var handler = new GetRelationShipsByUserIdQuery.GetRelationShipsByUserIdQueryHandler(_context);

            //Act 
            IQueryable<RelationShip> relationShips = await handler.Handle(new GetRelationShipsByUserIdQuery
            {
                Id = user.Id

            }, CancellationToken.None);

            //Assert
            relationShips.Should().BeEmpty();
        }
    }
}
