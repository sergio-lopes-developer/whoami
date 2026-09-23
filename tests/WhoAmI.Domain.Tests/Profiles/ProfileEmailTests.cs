using FluentAssertions;
using WhoAmI.Domain.Profiles.Events;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Factories;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileEmailTests {
    private static readonly DateTimeOffset _updatedAt =
        new(2026, 9, 23, 10, 30, 0, TimeSpan.Zero);

    [Fact]
    public void Profile_ShouldAllowEmailUpdate_WhenEmailIsValid() {
        // Arrange
        var profile = ProfileFactory.Create();
        var createdAt = profile.CreatedAt;
        var newEmail = Emails.CreateValid();

        // Act
        profile.UpdateEmail(newEmail, _updatedAt);

        // Assert
        profile.Email.Should().Be(newEmail);
        profile.CreatedAt.Should().Be(createdAt);
        profile.UpdatedAt.Should().Be(_updatedAt);
    }

    [Fact]
    public void Profile_ShouldNotAllowEmailUpdate_WhenEmailIsNull() {
        // Arrange
        var profile = ProfileFactory.Create();

        // Act
        var act =() => profile.UpdateEmail(null!, _updatedAt);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotSetUpdatedAt_WhenEmailIsUnchanged() {
        // Arrange
        var profile = ProfileFactory.CreateWithoutEvents();

        // Act
        profile.UpdateEmail(profile.Email, _updatedAt);

        // Assert
        profile.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Profile_ShouldNotRaiseDomainEvents_WhenEmailIsUnchanged() {
        // Arrange
        var profile = ProfileFactory.CreateWithoutEvents();

        // Act
        profile.UpdateEmail(profile.Email, _updatedAt);

        // Assert
        profile.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Profile_ShouldRaiseEmailUpdatedEvent_WhenEmailIsUpdated() {
        // Arrange
        var profile = ProfileFactory.CreateWithoutEvents();
        var newEmail = Emails.CreateValid();

        // Act
        profile.UpdateEmail(newEmail, _updatedAt);

        // Assert
        var domainEvent = profile.DomainEvents
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .BeOfType<EmailUpdatedEvent>()
            .Subject;

        domainEvent.ProfileId.Should().Be(profile.Id);
        domainEvent.NewEmail.Should().Be(newEmail.Address);

        profile.UpdatedAt.Should().Be(_updatedAt);
    }
}
