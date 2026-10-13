using FluentAssertions;
using WhoAmI.Domain.Profiles.Exceptions;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Helpers;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles.ValueObjects;

public class FirstNameTests {
    [Fact]
    public void FirstName_ShouldCreate_WhenInputIsValid() {
        // Arrange
        const string input = FirstNames.Sergio;

        // Act
        var firstName = new FirstName(input);

        // Assert
        firstName.Value.Should().Be(input);
    }

    [Fact]
    public void FirstName_ShouldPreserveInputCase_WhenCreated() {
        // Arrange
        const string canonicalInput = FirstNames.Sergio;
        var caseVariantInput = canonicalInput.ToUpperInvariant();

        // Act
        var firstName = new FirstName(caseVariantInput);

        // Assert
        firstName.Value.Should().Be(caseVariantInput);
    }

    [Fact]
    public void FirstName_ShouldThrow_WhenInputIsNull() {
        // Act
        var act = () => new FirstName(null!);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [MemberData(nameof(TextInputs.Blank), MemberType = typeof(TextInputs))]
    public void FirstName_ShouldThrow_WhenInputIsEmptyOrWhiteSpace(
        string input
    ) {
        // Act
        var act = () => new FirstName(input);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(FirstNames.Sergio + " ")]
    [InlineData(" " + FirstNames.Sergio)]
    [InlineData(" " + FirstNames.Sergio + " ")]
    public void FirstName_ShouldTrimLeadingAndTrailingWhiteSpace_WhenInputContainsWhiteSpace(
        string leadingOrTrailingWhiteSpaceInput
    ) {
        // Arrange
        const string canonicalInput = FirstNames.Sergio;

        // Act
        var firstName = new FirstName(leadingOrTrailingWhiteSpaceInput);

        // Assert
        firstName.Value.Should().Be(canonicalInput);
    }

    [Fact]
    public void FirstName_ShouldNormalizeInternalWhiteSpace_WhenInputContainsIrregularSpacing() {
        // Arrange
        const string canonicalInput = FirstNames.JohnChristopher;
        var irregularSpacingInput = canonicalInput.Replace(" ", "  ");

        // Act
        var firstName = new FirstName(irregularSpacingInput);

        // Assert
        firstName.Value.Should().Be(FirstNames.JohnChristopher);
    }

    [Fact]
    public void FirstName_ShouldCreate_WhenInputHasMinimumLength() {
        // Arrange
        var minLength = FirstName.LengthConstraint.MinLength;
        var minLengthInput = TextInputs.CreateStringWithLength(minLength);

        // Act
        var firstName = new FirstName(minLengthInput);

        // Assert
        firstName.Value.Should().Be(minLengthInput);
    }

    [Fact]
    public void FirstName_ShouldCreate_WhenInputHasMaximumLength() {
        // Arrange
        var maxLength = FirstName.LengthConstraint.MaxLength;
        var maxLengthInput = TextInputs.CreateStringWithLength(maxLength);

        // Act
        var firstName = new FirstName(maxLengthInput);

        // Assert
        firstName.Value.Should().Be(maxLengthInput);
    }

    [Fact]
    public void FirstName_ShouldThrow_WhenInputIsBelowMinimumLength() {
        // Arrange
        var invalidLength = FirstName.LengthConstraint.MinLength - 1;
        var tooShortInput = TextInputs.CreateStringWithLength(invalidLength);

        // Act
        var act = () => new FirstName(tooShortInput);

        // Assert
        act.Should().Throw<InvalidLengthException>();
    }

    [Fact]
    public void FirstName_ShouldThrow_WhenInputExceedsMaximumLength() {
        // Arrange
        var invalidLength = FirstName.LengthConstraint.MaxLength + 1;
        var tooLongInput = TextInputs.CreateStringWithLength(invalidLength);

        // Act
        var act = () => new FirstName(tooLongInput);

        // Assert
        act.Should().Throw<InvalidLengthException>();
    }

    [Fact]
    public void FirstName_ShouldBeEqual_WhenValuesAreEqual() {
        // Arrange
        const string input = FirstNames.Sergio;

        var firstName = new FirstName(input);
        var sameFirstName = new FirstName(input);

        // Assert
        firstName.Should().Be(sameFirstName);
        firstName.GetHashCode().Should().Be(sameFirstName.GetHashCode());

        (firstName == sameFirstName).Should().BeTrue();
        (firstName != sameFirstName).Should().BeFalse();
    }

    [Fact]
    public void FirstName_ShouldBeEqual_WhenValuesDifferOnlyInCase() {
        // Arrange
        const string input = FirstNames.Sergio;
        var variantInput = input.ToUpperInvariant();

        var canonicalFirstName = new FirstName(input);
        var caseVariant = new FirstName(variantInput);

        // Assert
        canonicalFirstName.Should().Be(caseVariant);
        canonicalFirstName.GetHashCode().Should().Be(caseVariant.GetHashCode());

        (canonicalFirstName == caseVariant).Should().BeTrue();
        (canonicalFirstName != caseVariant).Should().BeFalse();
    }

    [Fact]
    public void FirstName_ShouldBeEqual_WhenValuesDifferOnlyInLeadingOrTrailingWhiteSpace() {
        // Arrange
        const string input = FirstNames.JohnChristopher;
        const string variantInput = " " + input + " ";

        var canonicalFirstName = new FirstName(input);
        var leadingOrTrailingWhiteSpaceVariant = new FirstName(variantInput);

        // Assert
        canonicalFirstName.Should().Be(leadingOrTrailingWhiteSpaceVariant);

        canonicalFirstName.GetHashCode()
            .Should().Be(leadingOrTrailingWhiteSpaceVariant.GetHashCode());

        (canonicalFirstName == leadingOrTrailingWhiteSpaceVariant)
            .Should().BeTrue();

        (canonicalFirstName != leadingOrTrailingWhiteSpaceVariant)
            .Should().BeFalse();
    }

    [Fact]
    public void FirstName_ShouldBeEqual_WhenValuesDifferOnlyInInternalWhiteSpace() {
        // Arrange
        const string input = FirstNames.JohnChristopher;
        var variantInput = input.Replace(" ", "  ");

        var canonicalFirstName = new FirstName(input);
        var internalWhiteSpaceVariant = new FirstName(variantInput);

        // Assert
        canonicalFirstName.Should().Be(internalWhiteSpaceVariant);

        canonicalFirstName.GetHashCode()
            .Should().Be(internalWhiteSpaceVariant.GetHashCode());

        (canonicalFirstName == internalWhiteSpaceVariant).Should().BeTrue();
        (canonicalFirstName != internalWhiteSpaceVariant).Should().BeFalse();
    }

    [Fact]
    public void FirstName_ShouldBeEqual_WhenValuesDifferOnlyInCaseOrWhiteSpace() {
        // Arrange
        const string canonicalInput = FirstNames.JohnChristopher;

        const string leadingOrTrailingWhiteSpaceVariantInput =
            "  " + canonicalInput + "  ";

        var caseVariantInput = canonicalInput.ToUpperInvariant();
        var internalWhiteSpaceVariantInput = canonicalInput.Replace(" ", "  ");
        var canonicalFirstName = new FirstName(canonicalInput);
        var expectedHash = canonicalFirstName.GetHashCode();

        var variations = new[] {
            new FirstName(leadingOrTrailingWhiteSpaceVariantInput),
            new FirstName(caseVariantInput),
            new FirstName(internalWhiteSpaceVariantInput)
        };

        // Assert
        variations.Should().AllSatisfy(variant => {
            variant.Should().Be(canonicalFirstName);
            variant.GetHashCode().Should().Be(expectedHash);

            (variant == canonicalFirstName).Should().BeTrue();
            (variant != canonicalFirstName).Should().BeFalse();
        });
    }

    [Fact]
    public void FirstName_ShouldNotBeEqual_WhenValuesAreDifferent() {
        // Arrange
        var firstName = FirstNames.CreateSergio();
        var otherFirstName = FirstNames.CreateJohnChristopher();

        // Assert
        firstName.Should().NotBe(otherFirstName);

        (firstName != otherFirstName).Should().BeTrue();
        (firstName == otherFirstName).Should().BeFalse();
    }

    [Fact]
    public void FirstName_ShouldReturnValue_WhenConvertedToString() {
        // Arrange
        var firstName = FirstNames.CreateSergio();

        // Act
        var result = firstName.ToString();

        // Assert
        result.Should().Be(firstName.Value);
    }

    [Fact]
    public void FirstName_ShouldHaveExpectedLengthLimits() {
        // Assert
        FirstName.LengthConstraint.MinLength.Should().Be(2);
        FirstName.LengthConstraint.MaxLength.Should().Be(100);
    }

    [Theory]
    [InlineData("Sergio", true)]
    [InlineData("  Sergio  ", true)]
    [InlineData("John  Christopher", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("A", false)]
    public void LengthConstraint_ShouldValidateInputLength(
        string input,
        bool expected
    ) {
        // Act
        var result = FirstName.LengthConstraint.IsSatisfiedBy(input);

        // Assert
        result.Should().Be(expected);
    }
}
