using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Time;
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

        var newLinkedIn = "https://www.linkedin.com/in/new-username";

        var newGitHub = "https://github.com/new-username";

        var updatedAt = new DateTimeOffset(
            2026, 9, 23,
            10, 30, 0,
            TimeSpan.Zero
        );

        var repo = Substitute.For<IProfileRepository>();

        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(updatedAt);

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new UpdateSocialLinksCommandHandler(repo, clock);

        var command = new UpdateSocialLinksCommand(
            profile.Id,
            newLinkedIn,
            newGitHub
        );

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        profile.LinkedIn.Value.Should().Be(newLinkedIn);
        profile.GitHub.Value.Should().Be(newGitHub);
        profile.UpdatedAt.Should().Be(updatedAt);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateSocialLinks_ShouldReturnFailure_WhenProfileDoesNotExist() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var clock = Substitute.For<IClock>();

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Profile)null!);

        var handler = new UpdateSocialLinksCommandHandler(repo, clock);

        var id = Guid.NewGuid();

        var command = new UpdateSocialLinksCommand(
            id,
            "https://www.linkedin.com/in/username",
            "https://github.com/username"
        );

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFound");
        result.FirstError.Metadata!["ProfileId"].Should().Be(id);
    }
}
