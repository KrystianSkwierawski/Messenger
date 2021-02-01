using Application.IntegrationTests.Common;
using Application.Messages.Queries;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
            //Assert
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
                InvitedUserId = invitedUser.Id,
                InvitingUserId = invitingUser.Id
            };

            await _context.RelationShips.AddAsync(relationShip);
            await _context.SaveChangesAsync();

            Message message = new Message
            {
                RelationShipId = relationShip.Id
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            var handler = new GetMessagesByRelationShipIdQuery.GetMessagesByRelationShipIdQueryHandler(_context);

            //Act
            var messages = await handler.Handle(new GetMessagesByRelationShipIdQuery
            {
                RelationShipId = relationShip.Id
            }, CancellationToken.None);

            //Assert
            message.Should().NotBeNull();
            message.RelationShipId.Should().Be(relationShip.Id);
        }
    }
}
