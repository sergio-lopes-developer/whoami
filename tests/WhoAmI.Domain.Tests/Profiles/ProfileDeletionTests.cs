using FluentAssertions;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileDeletionTests {
    private static readonly DateTimeOffset _deletedAt =
        new(2026, 9, 29, 17, 25, 0, TimeSpan.Zero);

    [Fact]
    public void Profile_ShouldMarkAsDeleted_WhenDeleted() {
        // Arrange
        var profile = ProfileFactory.CreateWithoutEvents();

        // Act
        profile.Delete(_deletedAt);

        // Assert
        profile.IsDeleted.Should().BeTrue();
        profile.DeletedAt.Should().Be(_deletedAt);
    }

    [Fact]
    public void Profile_ShouldThrow_WhenProfileIsAlreadyDeleted() {
        // Arrange
        var profile = ProfileFactory.CreateWithoutEvents();
        profile.Delete(_deletedAt);

        // Act
        var act = () => profile.Delete(_deletedAt);

        // Assert
        act.Should().Throw<DomainException>();
    }
}
