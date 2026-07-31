using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.UpdateSocialLinks;
using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Application.Tests.Features.Profiles.UpdateSocialLinks;

public class UpdateSocialLinksCommandHandlerTests {
    [Fact]
    public async Task UpdateSocialLinks_ShouldUpdateSocialLinks_WhenLinksAreValid() {
        // Arrange
        var profile = ProfileFactory.Create();

        var newLinkedIn = "https://www.linkedin.com/in/username";

        var newGithub = "https://github.com/username";

        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new UpdateSocialLinksCommandHandler(repo);

        var command = new UpdateSocialLinksCommand(
            profile.Id,
            newLinkedIn,
            newGithub
        );

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        profile.LinkedIn.Value.Should().Be(newLinkedIn);
        profile.GitHub.Value.Should().Be(newGithub);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateSocialLinks_ShouldReturnFailure_WhenProfileDoesNotExist() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Profile)null!);

        var handler = new UpdateSocialLinksCommandHandler(repo);

        var id = Guid.NewGuid();

        var command = new UpdateSocialLinksCommand(
            id,
            "https://www.linkedin.com/in/username",
            "https://github.com/username"
        );

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFound");
        result.FirstError.Metadata!["ProfileId"].Should().Be(id);
    }
}
