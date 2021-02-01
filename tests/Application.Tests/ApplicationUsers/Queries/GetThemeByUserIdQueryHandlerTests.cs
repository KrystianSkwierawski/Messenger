using Application.ApplicationUsers.Queries;
using Application.IntegrationTests.Common;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.ApplicationUsers.Queries
{
    [Collection("QueryCollection")]
    public class GetThemeByUserIdQueryHandlerTests
    {
        private readonly Context _context;
        public GetThemeByUserIdQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task ShouldReturnThemeByUserId()
        {
            //Arrange
            ApplicationUser user = new ApplicationUser { Theme = "theme--default" };
            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            var handler = new GetThemeByUserIdQuery.GetThemeByUserIdQueryHandler(_context);

            //Act
            string userTheme = await handler.Handle(new GetThemeByUserIdQuery
            {
                UserId = user.Id
            }, CancellationToken.None);

            //Assert
            userTheme.Should().NotBeNullOrEmpty();
            userTheme.Should().Be(user.Theme);
            userTheme.GetType().Should().Be(typeof(string));
        }
    }
}
