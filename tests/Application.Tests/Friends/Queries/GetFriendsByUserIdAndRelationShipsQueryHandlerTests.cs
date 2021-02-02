using Application.Friends.Queries;
using Application.IntegrationTests.Common;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Friends.Queries.GetFriendsByUserIdAndRelationShipsQuery;

namespace Application.Tests.Friends.Queries
{
    [Collection("QueryCollection")]
    public class GetFriendsByUserIdAndRelationShipsQueryHandlerTests
    {
        private readonly Context _context;
        public GetFriendsByUserIdAndRelationShipsQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task ShouldReturnApplicationUsers()
        {
            //Arrage
            ApplicationUser currentUser = new ApplicationUser();
            ApplicationUser friendUser = new ApplicationUser();

            await _context.ApplicationUsers.AddAsync(currentUser);
            await _context.ApplicationUsers.AddAsync(friendUser);

            RelationShip relationShip = new RelationShip
            {
                InvitingUserId = currentUser.Id,
                InvitedUserId = friendUser.Id
            };

            await _context.RelationShips.AddAsync(relationShip);
            await _context.SaveChangesAsync();

            IQueryable<RelationShip> relationShips = _context.RelationShips;

            var handler = new GetFriendsByUserIdAndRelationShipsQueryHandler();

            //Act
            List<ApplicationUser> friends = await handler.Handle(new GetFriendsByUserIdAndRelationShipsQuery
            {
                Id = currentUser.Id,
                RelationShips = relationShips
            }, CancellationToken.None);

            //Assert
            friends.Should().NotBeNull();
            friends.FirstOrDefault(x => x.Id == friendUser.Id).Should().NotBeNull();
        }
    }
}
