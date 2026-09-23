using FluentAssertions;
using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileEqualityTests {
    private static readonly DateTimeOffset _createdAt =
        new(2026, 9, 22, 13, 25, 0, TimeSpan.Zero);

    [Fact]
    public void Profile_ShouldBeEqual_WhenIdsAreEqual() {
        // Arrange
        var id = Guid.NewGuid();

        var profile = new Profile(
            id,
            _createdAt,
            FullNames.CreateSergioLopes(),
            Emails.CreateSergio(),
            Urls.CreateLinkedIn(),
            Urls.CreateGitHub()
        );

        var sameProfile = new Profile(
            id,
            _createdAt,
            FullNames.CreateSergioLopes(),
            Emails.CreateSergio(),
            Urls.CreateLinkedIn(),
            Urls.CreateGitHub()
        );

        // Assert
        profile.Should().Be(sameProfile);
        profile.GetHashCode().Should().Be(sameProfile.GetHashCode());
        (profile == sameProfile).Should().BeTrue();
        (profile != sameProfile).Should().BeFalse();
    }

    [Fact]
    public void Profile_ShouldNotBeEqual_WhenIdsAreDifferent() {
        // Arrange
        var id = Guid.NewGuid();
        var otherId = Guid.NewGuid();

        var profile = new Profile(
            id,
            _createdAt,
            FullNames.CreateSergioLopes(),
            Emails.CreateSergio(),
            Urls.CreateLinkedIn(),
            Urls.CreateGitHub()
        );

        var otherProfile = new Profile(
            otherId,
            _createdAt,
            FullNames.CreateSergioLopes(),
            Emails.CreateSergio(),
            Urls.CreateLinkedIn(),
            Urls.CreateGitHub()
        );

        // Assert
        profile.Should().NotBe(otherProfile);
        (profile != otherProfile).Should().BeTrue();
        (profile == otherProfile).Should().BeFalse();
    }
}
