using FluentAssertions;
using WhoAmI.Domain.Profiles.Exceptions;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Helpers;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles.ValueObjects;

public class LastNameTests {
    [Fact]
    public void LastName_ShouldAllowCreation_WhenInputIsValid() {
        var input = LastNames.Lopes;

        var lastName = new LastName(input);

        lastName.Value.Should().Be(input);
    }

    [Fact]
    public void LastName_ShouldPreserveInputCase_WhenCreated() {
        var canonicalInput = LastNames.Lopes;
        var caseVariantInput = canonicalInput.ToUpperInvariant();

        var lastName = new LastName(caseVariantInput);

        lastName.Value.Should().Be(caseVariantInput);
    }

    [Fact]
    public void LastName_ShouldNotAllowCreation_WhenInputIsNull() {
        var act = () => new LastName(null!);

        act.Should().Throw<DomainException>();
    }

    [Theory]
    [MemberData(nameof(TextInputs.Blank), MemberType = typeof(TextInputs))]
    public void LastName_ShouldNotAllowCreation_WhenInputIsEmptyOrWhiteSpace(
        string input
    ) {
        var act = () => new LastName(input);

        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(LastNames.Lopes + " ")]
    [InlineData(" " + LastNames.Lopes)]
    [InlineData(" " + LastNames.Lopes + " ")]
    public void LastName_ShouldTrimLeadingOrTrailingWhiteSpace_WhenInputContainsIt(
        string leadingOrTrailingWhiteSpaceInput
    ) {
        var canonicalInput = LastNames.Lopes;

        var lastName = new LastName(leadingOrTrailingWhiteSpaceInput);

        lastName.Value.Should().Be(canonicalInput);
    }

    [Fact]
    public void LastName_ShouldNormalizeInternalWhiteSpace_WhenSpacingIsIrregular() {
        var canonicalInput = LastNames.LewisMiller;
        var irregularSpacingInput = canonicalInput.Replace(" ", "  ");

        var lastName = new LastName(irregularSpacingInput);

        lastName.Value.Should().Be(canonicalInput);
    }

    [Fact]
    public void LastName_ShouldAllowCreation_WhenInputIsAtMinLength() {
        var minLength = LastName.MinLength;
        var minLengthInput = TextInputs.CreateStringWithLength(minLength);

        var lastName = new LastName(minLengthInput);

        lastName.Value.Should().Be(minLengthInput);
    }

    [Fact]
    public void LastName_ShouldAllowCreation_WhenInputIsAtMaxLength() {
        var maxLength = LastName.MaxLength;
        var maxLengthInput = TextInputs.CreateStringWithLength(maxLength);

        var lastName = new LastName(maxLengthInput);

        lastName.Value.Should().Be(maxLengthInput);
    }

    [Fact]
    public void LastName_ShouldNotAllowCreation_WhenInputIsBelowMinLength() {
        var invalidLength = LastName.MinLength - 1;
        var tooShortInput = TextInputs.CreateStringWithLength(invalidLength);

        var act = () => new LastName(tooShortInput);

        act.Should().Throw<InvalidLengthException>();
    }

    [Fact]
    public void LastName_ShouldNotAllowCreation_WhenInputExceedsMaxLength() {
        var invalidLength = LastName.MaxLength + 1;
        var tooLongInput = TextInputs.CreateStringWithLength(invalidLength);

        var act = () => new LastName(tooLongInput);

        act.Should().Throw<InvalidLengthException>();
    }

    [Fact]
    public void LastName_ShouldBeEqual_WhenValuesAreIdentical() {
        var input = LastNames.Lopes;

        var lastName = new LastName(input);
        var sameLastName = new LastName(input);

        lastName.Should().Be(sameLastName);
        lastName.GetHashCode().Should().Be(sameLastName.GetHashCode());
        (lastName == sameLastName).Should().BeTrue();
        (lastName != sameLastName).Should().BeFalse();
    }

    [Fact]
    public void LastName_ShouldBeEqual_WhenValuesDifferOnlyInCase() {
        var input = LastNames.Lopes;
        var variantInput = input.ToUpperInvariant();

        var canonicalLastName = new LastName(input);
        var caseVariant = new LastName(variantInput);

        canonicalLastName.Should().Be(caseVariant);
        canonicalLastName.GetHashCode().Should().Be(caseVariant.GetHashCode());
        (canonicalLastName == caseVariant).Should().BeTrue();
        (canonicalLastName != caseVariant).Should().BeFalse();
    }

    [Fact]
    public void LastName_ShouldBeEqual_WhenValuesDifferOnlyInLeadingOrTrailingWhiteSpace() {
        var input = LastNames.LewisMiller;
        var variantInput = " " + input + " ";

        var canonicalLastName = new LastName(input);
        var leadingOrTrailingWhiteSpaceVariant = new LastName(variantInput);

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
        var input = LastNames.LewisMiller;
        var variantInput = input.Replace(" ", "  ");

        var canonicalLastName = new LastName(input);
        var internalWhiteSpaceVariant = new LastName(variantInput);

        canonicalLastName.Should().Be(internalWhiteSpaceVariant);
        canonicalLastName.GetHashCode()
            .Should().Be(internalWhiteSpaceVariant.GetHashCode());
        (canonicalLastName == internalWhiteSpaceVariant).Should().BeTrue();
        (canonicalLastName != internalWhiteSpaceVariant).Should().BeFalse();
    }

    [Fact]
    public void LastName_ShouldBeEqual_WhenValuesDifferOnlyInCaseOrWhiteSpace() {
        var canonicalInput = LastNames.LewisMiller;
        var leadingOrTrailingWhiteSpaceVariantInput =
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

        variations.Should().AllSatisfy(variant => {
            variant.Should().Be(canonicalLastName);
            variant.GetHashCode().Should().Be(expectedHash);
            (variant == canonicalLastName).Should().BeTrue();
            (variant != canonicalLastName).Should().BeFalse();
        });
    }

    [Fact]
    public void LastName_ShouldNotBeEqual_WhenValuesAreDifferent() {
        var canonicalLastName = LastNames.CreateLopes();
        var otherLastName = LastNames.CreateLewisMiller();

        canonicalLastName.Should().NotBe(otherLastName);
        (canonicalLastName != otherLastName).Should().BeTrue();
        (canonicalLastName == otherLastName).Should().BeFalse();
    }

    [Fact]
    public void LastName_LengthConstraint_ShouldExposeMinLength() {
        LastName.LengthConstraint.MinLength.Should().Be(LastName.MinLength);
    }

    [Fact]
    public void LastName_LengthConstraint_ShouldExposeMaxLength() {
        LastName.LengthConstraint.MaxLength.Should().Be(LastName.MaxLength);
    }

    [Theory]
    [InlineData("Lopes", true)]
    [InlineData("  Lopes  ", true)]
    [InlineData("Lewis Miller", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("A", false)]
    public void LastName_LengthConstraint_ShouldValidateInput(
        string input,
        bool expected
    ) {
        var result = LastName.LengthConstraint.IsSatisfiedBy(input);

        result.Should().Be(expected);
    }

    [Fact]
    public void LastName_ShouldReturnValue_WhenConvertedToString() {
        var lastName = LastNames.CreateLopes();

        var result = lastName.ToString();

        result.Should().Be(lastName.Value);
    }

    [Fact]
    public void LastName_LengthLimits_ShouldMatchDomainSpecification() {
        LastName.MinLength.Should().Be(2);
        LastName.MaxLength.Should().Be(100);
    }
}
