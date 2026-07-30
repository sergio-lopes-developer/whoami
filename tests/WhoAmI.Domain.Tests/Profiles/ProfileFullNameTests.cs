using FluentAssertions;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Factories;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles;

public class ProfileFullNameTests {
    [Fact]
    public void Profile_ShouldAllowFullNameUpdate_WhenFullNameIsValid() {
        var profile = ProfileFactory.Create();
        var newFullName = FullNames.CreateJohnChristopherLewisMiller();

        profile.UpdateFullName(newFullName);

        profile.FullName.Should().Be(newFullName);
    }

    [Fact]
    public void Profile_ShouldNotAllowFullNameUpdate_WhenFullNameIsNull() {
        var profile = ProfileFactory.Create();

        var act =() => profile.UpdateFullName(null!);

        act.Should().Throw<DomainException>();
    }
}
