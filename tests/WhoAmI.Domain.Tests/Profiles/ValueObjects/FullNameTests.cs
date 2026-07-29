using FluentAssertions;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles.ValueObjects;

public class FullNameTests {
    [Fact]
    public void FullName_ShouldAllowCreation_WhenParametersAreValid() {
        var firstName = FirstNames.CreateSergio();
        var lastName = LastNames.CreateLopes();

        var fullName = new FullName(firstName, lastName);

        fullName.FirstName.Should().Be(firstName);
        fullName.LastName.Should().Be(lastName);
    }

    [Fact]
    public void FullName_ShouldNotAllowCreation_WhenFirstNameIsNull() {
        var lastName = LastNames.CreateLopes();

        var act = () => new FullName(null!, lastName);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void FullName_ShouldNotAllowCreation_WhenLastNameIsNull() {
        var firstName = FirstNames.CreateSergio();

        var act = () => new FullName(firstName, null!);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void FullName_ShouldBeEqual_WhenComponentValuesAreIdentical() {
        var firstName = FirstNames.CreateSergio();
        var lastName = LastNames.CreateLopes();

        var fullName = new FullName(firstName, lastName);
        var sameFullName = new FullName(firstName, lastName);

        fullName.Should().Be(sameFullName);
        fullName.GetHashCode().Should().Be(sameFullName.GetHashCode());
        (fullName == sameFullName).Should().BeTrue();
        (fullName != sameFullName).Should().BeFalse();
    }

    [Fact]
    public void FullName_ShouldNotBeEqual_WhenComponentValuesAreDifferent() {
        var firstName = FirstNames.CreateSergio();
        var lastName = LastNames.CreateLopes();
        var otherFirstName = FirstNames.CreateJohnChristopher();
        var otherLastName = LastNames.CreateLewisMiller();

        var fullName = new FullName(firstName, lastName);
        var otherFullName = new FullName(otherFirstName, otherLastName);

        fullName.Should().NotBe(otherFullName);
        (fullName != otherFullName).Should().BeTrue();
        (fullName == otherFullName).Should().BeFalse();
    }

    [Fact]
    public void FullName_ShouldConcatenateFirstNameAndLastName_WhenConvertedToString() {
        var firstName = FirstNames.CreateSergio();
        var lastName = LastNames.CreateLopes();
        var fullName = new FullName(firstName, lastName);
        var expected = $"{firstName} {lastName}";

        var result = fullName.ToString();

        result.Should().Be(expected);
    }
}
