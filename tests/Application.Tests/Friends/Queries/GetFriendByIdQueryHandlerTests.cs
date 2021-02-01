﻿using Application.ApplicationUsers.Queries;
using Application.IntegrationTests.Common;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Friends.Queries
{
     [Collection("QueryCollection")]
    public class GetFriendByIdQueryHandlerTests
    {
        private readonly Context _context;
        public GetFriendByIdQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task ShouldReturnApplicationUser()
        {
            //Arrange
            ApplicationUser user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };

            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            var handler = new GetFriendByIdQuery.GetFriendByIdQueryHandler(_context);

            //Act
            var friend = await handler.Handle(new GetFriendByIdQuery
            {
                Id = user.Id
            }, CancellationToken.None);

            //Assert
            friend.Should().NotBeNull();
            friend.Id.Should().Be(user.Id);
        }

        [Fact]
        public async Task ShouldReturnEmpty()
        {
            //Arrange
            var handler = new GetFriendByIdQuery.GetFriendByIdQueryHandler(_context);

            //Act
            ApplicationUser friend = await handler.Handle(new GetFriendByIdQuery
            {
                Id = Guid.NewGuid().ToString()
            }, CancellationToken.None);

            //Assert
            friend.Should().BeNull();
        }
    }
}
