using FluentAssertions;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileDeletionTests {
    private static readonly DateTimeOffset _deletedAt =
        new(2026, 9, 29, 17, 25, 0, TimeSpan.Zero);

    [Fact]
    public void Profile_ShouldMarkProfileAsDeleted_WhenDeleteIsCalled() {
        // Arrange
        var profile = ProfileFactory.CreateWithoutEvents();

        // Act
        profile.Delete(_deletedAt);

        // Assert
        profile.IsDeleted.Should().BeTrue();
        profile.DeletedAt.Should().Be(_deletedAt);
    }
}
