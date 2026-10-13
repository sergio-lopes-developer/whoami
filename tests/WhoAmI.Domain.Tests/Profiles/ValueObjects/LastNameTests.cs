using FluentAssertions;
using WhoAmI.Domain.Profiles.Exceptions;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Helpers;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles.ValueObjects;

public class LastNameTests {
    [Fact]
    public void LastName_ShouldCreate_WhenInputIsValid() {
        // Arrange
        const string input = LastNames.Lopes;

        // Act
        var lastName = new LastName(input);

        // Assert
        lastName.Value.Should().Be(input);
    }

    [Fact]
    public void LastName_ShouldPreserveInputCase_WhenCreated() {
        // Arrange
        const string canonicalInput = LastNames.Lopes;
        var caseVariantInput = canonicalInput.ToUpperInvariant();

        // Act
        var lastName = new LastName(caseVariantInput);

        // Assert
        lastName.Value.Should().Be(caseVariantInput);
    }

    [Fact]
    public void LastName_ShouldThrow_WhenInputIsNull() {
        // Act
        var act = () => new LastName(null!);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [MemberData(nameof(TextInputs.Blank), MemberType = typeof(TextInputs))]
    public void LastName_ShouldThrow_WhenInputIsEmptyOrWhiteSpace(
        string input
    ) {
        // Act
        var act = () => new LastName(input);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(LastNames.Lopes + " ")]
    [InlineData(" " + LastNames.Lopes)]
    [InlineData(" " + LastNames.Lopes + " ")]
    public void LastName_ShouldTrimLeadingAndTrailingWhiteSpace_WhenInputContainsWhiteSpace(
        string leadingOrTrailingWhiteSpaceInput
    ) {
        // Arrange
        const string canonicalInput = LastNames.Lopes;

        // Act
        var lastName = new LastName(leadingOrTrailingWhiteSpaceInput);

        // Assert
        lastName.Value.Should().Be(canonicalInput);
    }

    [Fact]
    public void LastName_ShouldNormalizeInternalWhiteSpace_WhenInputContainsIrregularSpacing() {
        // Arrange
        const string canonicalInput = LastNames.LewisMiller;
        var irregularSpacingInput = canonicalInput.Replace(" ", "  ");

        // Act
        var lastName = new LastName(irregularSpacingInput);

        // Assert
        lastName.Value.Should().Be(canonicalInput);
    }

    [Fact]
    public void LastName_ShouldCreate_WhenInputHasMinimumLength() {
        // Arrange
        var minLength = LastName.LengthConstraint.MinLength;
        var minLengthInput = TextInputs.CreateStringWithLength(minLength);

        // Act
        var lastName = new LastName(minLengthInput);

        // Assert
        lastName.Value.Should().Be(minLengthInput);
    }

    [Fact]
    public void LastName_ShouldCreate_WhenInputHasMaximumLength() {
        // Arrange
        var maxLength = LastName.LengthConstraint.MaxLength;
        var maxLengthInput = TextInputs.CreateStringWithLength(maxLength);

        // Act
        var lastName = new LastName(maxLengthInput);

        // Assert
        lastName.Value.Should().Be(maxLengthInput);
    }

    [Fact]
    public void LastName_ShouldThrow_WhenInputIsBelowMinimumLength() {
        // Arrange
        var invalidLength = LastName.LengthConstraint.MinLength - 1;
        var tooShortInput = TextInputs.CreateStringWithLength(invalidLength);

        // Act
        var act = () => new LastName(tooShortInput);

        // Assert
        act.Should().Throw<InvalidLengthException>();
    }

    [Fact]
    public void LastName_ShouldThrow_WhenInputExceedsMaximumLength() {
        // Arrange
        var invalidLength = LastName.LengthConstraint.MaxLength + 1;
        var tooLongInput = TextInputs.CreateStringWithLength(invalidLength);

        // Act
        var act = () => new LastName(tooLongInput);

        // Assert
        act.Should().Throw<InvalidLengthException>();
    }

    [Fact]
    public void LastName_ShouldBeEqual_WhenValuesAreEqual() {
        // Arrange
        const string input = LastNames.Lopes;

        var lastName = new LastName(input);
        var sameLastName = new LastName(input);

        // Assert
        lastName.Should().Be(sameLastName);
        lastName.GetHashCode().Should().Be(sameLastName.GetHashCode());

        (lastName == sameLastName).Should().BeTrue();
        (lastName != sameLastName).Should().BeFalse();
    }

    [Fact]
    public void LastName_ShouldBeEqual_WhenValuesDifferOnlyInCase() {
        // Arrange
        const string input = LastNames.Lopes;
        var variantInput = input.ToUpperInvariant();

        var canonicalLastName = new LastName(input);
        var caseVariant = new LastName(variantInput);

        // Assert
        canonicalLastName.Should().Be(caseVariant);
        canonicalLastName.GetHashCode().Should().Be(caseVariant.GetHashCode());

        (canonicalLastName == caseVariant).Should().BeTrue();
        (canonicalLastName != caseVariant).Should().BeFalse();
    }

    [Fact]
    public void LastName_ShouldBeEqual_WhenValuesDifferOnlyInLeadingOrTrailingWhiteSpace() {
        // Arrange
        const string input = LastNames.LewisMiller;
        const string variantInput = " " + input + " ";

        var canonicalLastName = new LastName(input);
        var leadingOrTrailingWhiteSpaceVariant = new LastName(variantInput);

        // Assert
        canonicalLastName.Should().Be(leadingOrTrailingWhiteSpaceVariant);

        canonicalLastName.GetHashCode()
            .Should().Be(leadingOrTrailingWhiteSpaceVariant.GetHashCode());

        (canonicalLastName == leadingOrTrailingWhiteSpaceVariant)
            .Should().BeTrue();

        (canonicalLastName != leadingOrTrailingWhiteSpaceVariant)
            .Should().BeFalse();
    }

    [Fact]
    public void LastName_ShouldBeEqual_WhenValuesDifferOnlyInInternalWhiteSpace() {
        // Arrange
        const string input = LastNames.LewisMiller;
        var variantInput = input.Replace(" ", "  ");

        var canonicalLastName = new LastName(input);
        var internalWhiteSpaceVariant = new LastName(variantInput);

        // Assert
        canonicalLastName.Should().Be(internalWhiteSpaceVariant);

        canonicalLastName.GetHashCode()
            .Should().Be(internalWhiteSpaceVariant.GetHashCode());

        (canonicalLastName == internalWhiteSpaceVariant).Should().BeTrue();
        (canonicalLastName != internalWhiteSpaceVariant).Should().BeFalse();
    }

    [Fact]
    public void LastName_ShouldBeEqual_WhenValuesDifferOnlyInCaseOrWhiteSpace() {
        // Arrange
        const string canonicalInput = LastNames.LewisMiller;

        const string leadingOrTrailingWhiteSpaceVariantInput =
            "  " + canonicalInput + "  ";

        var caseVariantInput = canonicalInput.ToUpperInvariant();
        var internalWhiteSpaceVariantInput = canonicalInput.Replace(" ", "  ");
        var canonicalLastName = new LastName(canonicalInput);
        var expectedHash = canonicalLastName.GetHashCode();

        var variations = new[] {
            new LastName(leadingOrTrailingWhiteSpaceVariantInput),
            new LastName(caseVariantInput),
            new LastName(internalWhiteSpaceVariantInput)
        };

        // Assert
        variations.Should().AllSatisfy(variant => {
            variant.Should().Be(canonicalLastName);
            variant.GetHashCode().Should().Be(expectedHash);

            (variant == canonicalLastName).Should().BeTrue();
            (variant != canonicalLastName).Should().BeFalse();
        });
    }

    [Fact]
    public void LastName_ShouldNotBeEqual_WhenValuesAreDifferent() {
        // Arrange
        var canonicalLastName = LastNames.CreateLopes();
        var otherLastName = LastNames.CreateLewisMiller();

        // Assert
        canonicalLastName.Should().NotBe(otherLastName);

        (canonicalLastName != otherLastName).Should().BeTrue();
        (canonicalLastName == otherLastName).Should().BeFalse();
    }

    [Fact]
    public void LastName_ShouldReturnValue_WhenConvertedToString() {
        // Arrange
        var lastName = LastNames.CreateLopes();

        // Act
        var result = lastName.ToString();

        // Assert
        result.Should().Be(lastName.Value);
    }

    [Fact]
    public void LastName_ShouldHaveExpectedLengthLimits() {
        // Assert
        LastName.LengthConstraint.MinLength.Should().Be(2);
        LastName.LengthConstraint.MaxLength.Should().Be(100);
    }

    [Theory]
    [InlineData("Lopes", true)]
    [InlineData("  Lopes  ", true)]
    [InlineData("Lewis Miller", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("A", false)]
    public void LengthConstraint_ShouldValidateInputLength(
        string input,
        bool expected
    ) {
        // Act
        var result = LastName.LengthConstraint.IsSatisfiedBy(input);

        // Assert
        result.Should().Be(expected);
    }
}
