using FluentAssertions;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Factories;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileSocialLinksTests {
    private static readonly DateTimeOffset _updatedAt =
        new(2026, 9, 23, 10, 30, 0, TimeSpan.Zero);

    [Fact]
    public void Profile_ShouldAllowSocialLinksUpdate_WhenSocialLinksAreValid() {
        // Arrange
        var profile = ProfileFactory.Create();
        var createdAt = profile.CreatedAt;
        var newLinkedIn = Urls.CreateExample();
        var newGitHub = Urls.CreateExample();

        // Act
        profile.UpdateSocialLinks(newLinkedIn, newGitHub, _updatedAt);

        // Assert
        profile.LinkedIn.Should().Be(newLinkedIn);
        profile.GitHub.Should().Be(newGitHub);
        profile.CreatedAt.Should().Be(createdAt);
        profile.UpdatedAt.Should().Be(_updatedAt);
    }

    [Fact]
    public void Profile_ShouldNotAllowSocialLinksUpdate_WhenLinkedInIsNull() {
        // Arrange
        var profile = ProfileFactory.Create();
        var newGitHub = Urls.CreateExample();

        // Act
        var act =() => profile.UpdateSocialLinks(null!, newGitHub, _updatedAt);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotAllowSocialLinksUpdate_WhenGitHubIsNull() {
        // Arrange
        var profile = ProfileFactory.Create();
        var newLinkedIn = Urls.CreateExample();

        // Act
        var act =() => profile.UpdateSocialLinks(
            newLinkedIn,
            null!,
            _updatedAt
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotSetUpdatedAt_WhenSocialLinksAreUnchanged() {
        // Arrange
        var profile = ProfileFactory.CreateWithoutEvents();

        // Act
        profile.UpdateSocialLinks(profile.LinkedIn, profile.GitHub, _updatedAt);

        // Assert
        profile.UpdatedAt.Should().BeNull();
    }
}
