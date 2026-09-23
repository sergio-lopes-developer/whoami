using FluentAssertions;
using WhoAmI.Domain.Profiles;
using WhoAmI.Domain.Profiles.Events;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Factories;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileCreationTests {
    private static readonly DateTimeOffset _createdAt =
        new(2026, 9, 22, 13, 25, 0, TimeSpan.Zero);

    [Fact]
    public void Profile_ShouldAllowCreation_WhenParametersAreValid() {
        // Arrange
        var fullName = FullNames.CreateSergioLopes();
        var email = Emails.CreateSergio();
        var linkedIn = Urls.CreateLinkedIn();
        var gitHub = Urls.CreateGitHub();

        // Act
        var profile = Profile.Create(
            _createdAt,
            fullName,
            email,
            linkedIn,
            gitHub
        );

        // Assert
        profile.Id.Should().NotBeEmpty();
        profile.FullName.Should().Be(fullName);
        profile.Email.Should().Be(email);
        profile.LinkedIn.Should().Be(linkedIn);
        profile.GitHub.Should().Be(gitHub);
        profile.CreatedAt.Should().Be(_createdAt);
        profile.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Profile_ShouldNotAllowCreation_WhenFullNameIsNull() {
        // Arrange
        var email = Emails.CreateSergio();
        var linkedIn = Urls.CreateLinkedIn();
        var gitHub = Urls.CreateGitHub();

        // Act
        var act = () => Profile.Create(
            _createdAt,
            null!,
            email,
            linkedIn,
            gitHub
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotAllowCreation_WhenEmailIsNull() {
        // Arrange
        var fullName = FullNames.CreateSergioLopes();
        var linkedIn = Urls.CreateLinkedIn();
        var gitHub = Urls.CreateGitHub();

        // Act
        var act = () => Profile.Create(
            _createdAt,
            fullName,
            null!,
            linkedIn,
            gitHub
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotAllowCreation_WhenLinkedInIsNull() {
        // Arrange
        var fullName = FullNames.CreateSergioLopes();
        var email = Emails.CreateSergio();
        var gitHub = Urls.CreateGitHub();

        // Act
        var act = () => Profile.Create(
            _createdAt,
            fullName,
            email,
            null!,
            gitHub
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotAllowCreation_WhenGitHubIsNull() {
        // Arrange
        var fullName = FullNames.CreateSergioLopes();
        var email = Emails.CreateSergio();
        var linkedIn = Urls.CreateLinkedIn();

        // Act
        var act = () => Profile.Create(
            _createdAt,
            fullName,
            email,
            linkedIn,
            null!
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldRaiseProfileCreatedEvent_WhenProfileIsCreated() {
        // Arrange
        var profile = ProfileFactory.Create();

        // Assert
        var domainEvent = profile.DomainEvents
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .BeOfType<ProfileCreatedEvent>()
            .Subject;

        domainEvent.ProfileId.Should().Be(profile.Id);
    }
}
