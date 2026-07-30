using FluentAssertions;
using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileEqualityTests {
    [Fact]
    public void Profile_ShouldBeEqual_WhenIdsAreEqual() {
        var id = Guid.NewGuid();

        var profile = new Profile(
            id,
            FullNames.CreateSergioLopes(),
            Emails.CreateSergio(),
            Urls.CreateLinkedIn(),
            Urls.CreateGitHub()
        );
        var sameProfile = new Profile(
            id,
            FullNames.CreateSergioLopes(),
            Emails.CreateSergio(),
            Urls.CreateLinkedIn(),
            Urls.CreateGitHub()
        );

        profile.Should().Be(sameProfile);
        profile.GetHashCode().Should().Be(sameProfile.GetHashCode());
        (profile == sameProfile).Should().BeTrue();
        (profile != sameProfile).Should().BeFalse();
    }

    [Fact]
    public void Profile_ShouldNotBeEqual_WhenIdsAreDifferent() {
        var id = Guid.NewGuid();
        var otherId = Guid.NewGuid();

        var profile = new Profile(
            id,
            FullNames.CreateSergioLopes(),
            Emails.CreateSergio(),
            Urls.CreateLinkedIn(),
            Urls.CreateGitHub()
        );
        var otherProfile = new Profile(
            otherId,
            FullNames.CreateSergioLopes(),
            Emails.CreateSergio(),
            Urls.CreateLinkedIn(),
            Urls.CreateGitHub()
        );

        profile.Should().NotBe(otherProfile);
        (profile != otherProfile).Should().BeTrue();
        (profile == otherProfile).Should().BeFalse();
    }
}
