using Application.ApplicationUsers.Queries;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Messenger.Application.IntegrationTests.ApplicationUsers.Queries
{
    using static Testing;
    public class GetThemeByUserIdTests : TestBase
    {
        [Test]
        public async Task ShouldReturnUserTheme()
        {
            // Arrange
            ApplicationUser user = await CreateUserAsync("User1", "Testing1234!", new string[] { });


            // Act
            var query = new GetThemeByUserIdQuery
            {
                UserId = user.Id
            };

            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(user.Theme);
        }
    }
}
