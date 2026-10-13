using FluentAssertions;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles.ValueObjects;

public class FullNameTests {
    [Fact]
    public void FullName_ShouldCreate_WhenParametersAreValid() {
        // Arrange
        var firstName = FirstNames.CreateSergio();
        var lastName = LastNames.CreateLopes();

        // Act
        var fullName = new FullName(firstName, lastName);

        // Assert
        fullName.FirstName.Should().Be(firstName);
        fullName.LastName.Should().Be(lastName);
    }

    [Fact]
    public void FullName_ShouldThrow_WhenFirstNameIsNull() {
        // Arrange
        var lastName = LastNames.CreateLopes();

        // Act
        var act = () => new FullName(null!, lastName);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void FullName_ShouldThrow_WhenLastNameIsNull() {
        // Arrange
        var firstName = FirstNames.CreateSergio();

        // Act
        var act = () => new FullName(firstName, null!);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void FullName_ShouldBeEqual_WhenComponentValuesAreEqual() {
        // Arrange
        var firstName = FirstNames.CreateSergio();
        var lastName = LastNames.CreateLopes();

        var fullName = new FullName(firstName, lastName);
        var sameFullName = new FullName(firstName, lastName);

        // Assert
        fullName.Should().Be(sameFullName);
        fullName.GetHashCode().Should().Be(sameFullName.GetHashCode());

        (fullName == sameFullName).Should().BeTrue();
        (fullName != sameFullName).Should().BeFalse();
    }

    [Fact]
    public void FullName_ShouldNotBeEqual_WhenComponentValuesAreDifferent() {
        // Arrange
        var firstName = FirstNames.CreateSergio();
        var lastName = LastNames.CreateLopes();
        var otherFirstName = FirstNames.CreateJohnChristopher();
        var otherLastName = LastNames.CreateLewisMiller();

        var fullName = new FullName(firstName, lastName);
        var otherFullName = new FullName(otherFirstName, otherLastName);

        // Assert
        fullName.Should().NotBe(otherFullName);

        (fullName != otherFullName).Should().BeTrue();
        (fullName == otherFullName).Should().BeFalse();
    }

    [Fact]
    public void FullName_ShouldConcatenateFirstNameAndLastName_WhenConvertedToString() {
        // Arrange
        var firstName = FirstNames.CreateSergio();
        var lastName = LastNames.CreateLopes();
        var fullName = new FullName(firstName, lastName);
        var expected = $"{firstName} {lastName}";

        // Act
        var result = fullName.ToString();

        // Assert
        result.Should().Be(expected);
    }
}
