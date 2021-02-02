using Application.IntegrationTests.Common;
using Application.Messages.Queries;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Messages.Queries.GetMessagesByRelationShipIdQuery;

namespace Application.Tests.Messages.Queries
{
    [Collection("QueryCollection")]
    public class GetMessagesByRelationShipIdQueryHandlerTests
    {
        private readonly Context _context;
        public GetMessagesByRelationShipIdQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task ShouldReturnMessageByRelationShipId()
        {
            //Arrange
            ApplicationUser invitedUser = new ApplicationUser();
            ApplicationUser invitingUser = new ApplicationUser();

            await _context.ApplicationUsers.AddAsync(invitedUser);
            await _context.ApplicationUsers.AddAsync(invitingUser);

            RelationShip relationShip = new RelationShip
            {
                InvitedUserId = invitedUser.Id,
                InvitingUserId = invitingUser.Id
            };

            await _context.RelationShips.AddAsync(relationShip);

            Message message = new Message
            {
                RelationShipId = relationShip.Id
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            var handler = new GetMessagesByRelationShipIdQueryHandler(_context);

            //Act
            List<Message> messages = await handler.Handle(new GetMessagesByRelationShipIdQuery
            {
                RelationShipId = relationShip.Id
            }, CancellationToken.None);

            //Assert
            messages.Should().NotBeNull();
            messages.FirstOrDefault(x => x.RelationShipId == relationShip.Id).Should().NotBeNull();
        }
    }
}
