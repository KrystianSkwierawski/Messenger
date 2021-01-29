using Application.ApplicationUsers.Queries;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Messenger.Application.IntegrationTests.Friends.Queries
{
    using static Testing;
    public class GetFriendByIdQueryTests : TestBase
    {
        [Test]
        public async Task ShouldReturnFriend()
        {
            ApplicationUser user = await CreateUserAsync("test@local", "Testing1234!", new string[] { });

            var query = new GetFriendByIdQuery
            {
                Id = user.Id
            };

            var result = await SendAsync(query);

            result.Should().NotBeNull();
        }
    }
}