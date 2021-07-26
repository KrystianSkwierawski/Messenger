using Application.ApplicationUsers.Commands;
using Application.UnitTests.Common;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.ApplicationUsers.Commands.UpdateThemeByUserIdCommand;

namespace Application.Tests.ApplicationUsers.Commands
{
    public class UpdateThemeByUserIdCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task ShouldUpdateApplicationUserTheme()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser { Theme = Theme.Default };

            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            var handler = new UpdateThemeByUserIdCommandHandler(_context);

            // Act
            var result = await handler.Handle(new UpdateThemeByUserIdCommand
            {
                UserId = user.Id,
                Theme = Theme.Light
            }, CancellationToken.None);

            // Assert
            _context.ApplicationUsers.Find(user.Id).Theme.Should().Be(Theme.Light);
        }
    }
}
