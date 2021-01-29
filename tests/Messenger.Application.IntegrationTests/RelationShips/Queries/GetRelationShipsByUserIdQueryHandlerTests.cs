using Application.RelationShips.Queries;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Messenger.Application.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Messenger.Application.IntegrationTests.RelationShips.Queries
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
            var rel = new RelationShip
            {
                IsAccepted = true,
                InvitedUserId = Guid.NewGuid().ToString(),
                InvitingUserId = Guid.NewGuid().ToString(),
            };

            await _context.AddAsync(rel);

            await _context.SaveChangesAsync();

                var handler = new GetRelationShipsByUserIdQuery.GetRelationShipsByUserIdQueryHandler(_context);
            var result = await handler.Handle(new GetRelationShipsByUserIdQuery { Id = rel.InvitingUserId}, CancellationToken.None);

            result.Should().NotBeNull();
            result.First().Should().NotBeNull();
        }
    }
}
