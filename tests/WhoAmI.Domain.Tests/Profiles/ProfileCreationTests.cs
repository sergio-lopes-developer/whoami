using FluentAssertions;
using WhoAmI.Domain.Profiles;
using WhoAmI.Domain.Profiles.Events;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Factories;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileCreationTests {
    [Fact]
    public void Profile_ShouldAllowCreation_WhenParametersAreValid() {
        var fullName = FullNames.CreateSergioLopes();
        var email = Emails.CreateSergio();
        var linkedIn = Urls.CreateLinkedIn();
        var gitHub = Urls.CreateGitHub();

        var profile = Profile.Create(fullName, email, linkedIn, gitHub);

        profile.Id.Should().NotBeEmpty();
        profile.FullName.Should().Be(fullName);
        profile.Email.Should().Be(email);
        profile.LinkedIn.Should().Be(linkedIn);
        profile.GitHub.Should().Be(gitHub);
    }

    [Fact]
    public void Profile_ShouldNotAllowCreation_WhenFullNameIsNull() {
        var email = Emails.CreateSergio();
        var linkedIn = Urls.CreateLinkedIn();
        var gitHub = Urls.CreateGitHub();

        var act = () => Profile.Create(null!, email, linkedIn, gitHub);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotAllowCreation_WhenEmailIsNull() {
        var fullName = FullNames.CreateSergioLopes();
        var linkedIn = Urls.CreateLinkedIn();
        var gitHub = Urls.CreateGitHub();

        var act = () => Profile.Create(fullName, null!, linkedIn, gitHub);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotAllowCreation_WhenLinkedInIsNull() {
        var fullName = FullNames.CreateSergioLopes();
        var email = Emails.CreateSergio();
        var gitHub = Urls.CreateGitHub();

        var act = () => Profile.Create(fullName, email, null!, gitHub);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotAllowCreation_WhenGitHubIsNull() {
        var fullName = FullNames.CreateSergioLopes();
        var email = Emails.CreateSergio();
        var linkedIn = Urls.CreateLinkedIn();

        var act = () => Profile.Create(fullName, email, linkedIn, null!);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldRaiseProfileCreatedEvent_WhenProfileIsCreated() {
        var profile = ProfileFactory.Create();

        profile.DomainEvents
            .Should().ContainSingle(e => e is ProfileCreatedEvent);
        var domainEvent =
            profile.DomainEvents.OfType<ProfileCreatedEvent>().Single();
        domainEvent.ProfileId.Should().Be(profile.Id);
    }
}
