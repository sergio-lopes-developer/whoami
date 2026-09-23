using FluentAssertions;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Factories;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileFullNameTests {
    private static readonly DateTimeOffset _updatedAt =
        new(2026, 9, 23, 10, 30, 0, TimeSpan.Zero);

    [Fact]
    public void Profile_ShouldAllowFullNameUpdate_WhenFullNameIsValid() {
        // Arrange
        var profile = ProfileFactory.Create();
        var createdAt = profile.CreatedAt;
        var newFullName = FullNames.CreateJohnChristopherLewisMiller();

        // Act
        profile.UpdateFullName(newFullName, _updatedAt);

        // Assert
        profile.FullName.Should().Be(newFullName);
        profile.CreatedAt.Should().Be(createdAt);
        profile.UpdatedAt.Should().Be(_updatedAt);
    }

    [Fact]
    public void Profile_ShouldNotAllowFullNameUpdate_WhenFullNameIsNull() {
        // Arrange
        var profile = ProfileFactory.Create();

        // Act
        var act =() => profile.UpdateFullName(null!, _updatedAt);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Profile_ShouldNotSetUpdatedAt_WhenFullNameIsUnchanged() {
        // Arrange
        var profile = ProfileFactory.CreateWithoutEvents();

        // Act
        profile.UpdateFullName(profile.FullName, _updatedAt);

        // Assert
        profile.UpdatedAt.Should().BeNull();
    }
}
