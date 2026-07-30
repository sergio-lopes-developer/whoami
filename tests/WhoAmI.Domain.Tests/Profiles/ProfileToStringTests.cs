using FluentAssertions;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileToStringTests {
    [Fact]
    public void Profile_ShouldReturnFullName_WhenConvertedToString() {
        var profile = ProfileFactory.Create();
        var expected = $"Profile - {profile.FullName}";

        var result = profile.ToString();

        result.Should().Be(expected);
    }
}
