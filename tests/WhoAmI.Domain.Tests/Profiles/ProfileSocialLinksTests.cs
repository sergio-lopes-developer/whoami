using FluentAssertions;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Factories;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileSocialLinksTests {
    [Fact]
    public void Profile_ShouldAllowSocialLinksUpdate_WhenSocialLinksAreValid() {
        var profile = ProfileFactory.Create();
        var newLinkedIn = Urls.CreateExample();
        var newGitHub = Urls.CreateExample();

        profile.UpdateSocialLinks(newLinkedIn, newGitHub);

        profile.LinkedIn.Should().Be(newLinkedIn);
        profile.GitHub.Should().Be(newGitHub);
    }

    [Fact]
    public void Profile_ShouldNotAllowSocialLinksUpdate_WhenLinkedInIsNull() {
        var profile = ProfileFactory.Create();
        var newGitHub = Urls.CreateExample();

        var act =() => profile.UpdateSocialLinks(null!, newGitHub);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotAllowSocialLinksUpdate_WhenGitHubIsNull() {
        var profile = ProfileFactory.Create();
        var newLinkedIn = Urls.CreateExample();

        var act =() => profile.UpdateSocialLinks(newLinkedIn, null!);

        act.Should().Throw<DomainException>();
    }
}
