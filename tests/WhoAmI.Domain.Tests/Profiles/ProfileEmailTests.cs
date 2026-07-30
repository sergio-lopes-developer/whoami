using FluentAssertions;
using WhoAmI.Domain.Profiles.Events;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Factories;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileEmailTests {
    [Fact]
    public void Profile_ShouldAllowEmailUpdate_WhenEmailIsValid() {
        var profile = ProfileFactory.Create();
        var newEmail = Emails.CreateValid();

        profile.UpdateEmail(newEmail);

        profile.Email.Should().Be(newEmail);
    }

    [Fact]
    public void Profile_ShouldNotAllowEmailUpdate_WhenEmailIsNull() {
        var profile = ProfileFactory.Create();

        var act =() => profile.UpdateEmail(null!);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldRaiseEmailUpdatedEvent_WhenEmailUpdated() {
        var profile = ProfileFactory.CreateWithoutEvents();
        var newEmail = Emails.CreateValid();

        profile.UpdateEmail(newEmail);

        profile.DomainEvents
            .Should().ContainSingle(e => e is EmailUpdatedEvent);
        var domainEvent =
            profile.DomainEvents.OfType<EmailUpdatedEvent>().Single();
        domainEvent.ProfileId.Should().Be(profile.Id);
        domainEvent.NewEmail.Should().Be(newEmail.Address);
    }

    [Fact]
    public void Profile_ShouldNotRaiseEmailUpdatedEvent_WhenEmailIsSame() {
        var profile = ProfileFactory.CreateWithoutEvents();
        var newEmail = Emails.CreateSergio();

        profile.UpdateEmail(newEmail);

        profile.DomainEvents.Should().NotContain(e => e is EmailUpdatedEvent);
    }
}
