using FluentAssertions;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileToStringTests {
    [Fact]
    public void Profile_ShouldIncludeFullName_WhenConvertedToString() {
        // Arrange
        var profile = ProfileFactory.Create();
        var expected = $"Profile - {profile.FullName}";

        // Act
        var result = profile.ToString();

        // Assert
        result.Should().Be(expected);
    }
}
