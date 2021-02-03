using Application.ApplicationUsers.Commands;
using Application.Tests.Common;
using Domain.Entities;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.ApplicationUsers.Commands.UpdateImageUrlByUserIdCommand;

namespace Application.Tests.ApplicationUsers.Commands
{
    public class UpdateImageUrlByUserIdCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task ShouldUpdateApplicationUserImageUrl()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser { ImageUrl = "ImageUrlIsNotUpdated" };
            string expectedImageUrl = "ImageUrlIsUpdated";

            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            var handler = new UpdateImageUrlByUserIdCommandHandler(_context);

            // Act
            var result = await handler.Handle(new UpdateImageUrlByUserIdCommand
            {
                UserId = user.Id,
                ImageUrl = expectedImageUrl
            }, CancellationToken.None);

            // Assert
            _context.ApplicationUsers.Find(user.Id).ImageUrl.Should().Be(expectedImageUrl);
        }
    }
}
