using Application.ApplicationUsers.Commands;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.UnitTests.Common;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.ApplicationUsers.Commands.UpdateImageByUserIdCommand;

namespace Application.Tests.ApplicationUsers.Commands
{
    public class UpdateImageByUserIdCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task ShouldUpdateApplicationUserImageUrl()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser { ImageUrl = "ImageUrlIsNotUpdated" };
            ImageFile imageFile = new ImageFile
            {
                FileName = "ImageUrlIsUpdated",
            };
            Mock<IImageFileBuilder> imageFileBuliderMock = new Mock<IImageFileBuilder>();

            await _context.ApplicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            var handler = new UpdateImageByUserIdCommandHandler(_context, imageFileBuliderMock.Object);

            // Act
            var result = await handler.Handle(new UpdateImageByUserIdCommand
            {
                UserId = user.Id,
                ImageFile = imageFile
            }, CancellationToken.None);

            // Assert
            string expectedImageUrl = @"\images\avatars\" + imageFile.FileName;
            _context.ApplicationUsers.Find(user.Id).ImageUrl.Should().Be(expectedImageUrl);
        }
    }
}
