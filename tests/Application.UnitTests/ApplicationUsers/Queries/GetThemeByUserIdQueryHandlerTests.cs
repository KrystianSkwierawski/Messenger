using Application.ApplicationUsers.Queries;
using Application.UnitTests.Common;
using Domain.Entities;
using Domain.Common;
using FluentAssertions;
using Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.ApplicationUsers.Queries.GetThemeByUserIdQuery;

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
            ApplicationUser user = new ApplicationUser { Theme = Theme.Default };
            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            var handler = new GetThemeByUserIdQueryHandler(_context);

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
