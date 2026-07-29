using FluentAssertions;
using WhoAmI.Domain.Profiles.Exceptions;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Helpers;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles.ValueObjects;

public class FirstNameTests {
    [Fact]
    public void FirstName_ShouldAllowCreation_WhenInputIsValid() {
        var input = FirstNames.Sergio;

        var firstName = new FirstName(input);

        firstName.Value.Should().Be(input);
    }

    [Fact]
    public void FirstName_ShouldPreserveInputCase_WhenCreated() {
        var canonicalInput = FirstNames.Sergio;
        var caseVariantInput = canonicalInput.ToUpperInvariant();

        var firstName = new FirstName(caseVariantInput);

        firstName.Value.Should().Be(caseVariantInput);
    }

    [Fact]
    public void FirstName_ShouldNotAllowCreation_WhenInputIsNull() {
        var act = () => new FirstName(null!);

        act.Should().Throw<DomainException>();
    }

    [Theory]
    [MemberData(nameof(TextInputs.Blank), MemberType = typeof(TextInputs))]
    public void FirstName_ShouldNotAllowCreation_WhenInputIsEmptyOrWhiteSpace(
        string input
    ) {
        var act = () => new FirstName(input);

        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(FirstNames.Sergio + " ")]
    [InlineData(" " + FirstNames.Sergio)]
    [InlineData(" " + FirstNames.Sergio + " ")]
    public void FirstName_ShouldTrimLeadingOrTrailingWhiteSpace_WhenInputContainsIt(
        string leadingOrTrailingWhiteSpaceInput
    ) {
        var canonicalInput = FirstNames.Sergio;

        var firstName = new FirstName(leadingOrTrailingWhiteSpaceInput);

        firstName.Value.Should().Be(canonicalInput);
    }

    [Fact]
    public void FirstName_ShouldNormalizeInternalWhiteSpace_WhenSpacingIsIrregular() {
        var canonicalInput = FirstNames.JohnChristopher;
        var irregularSpacingInput = canonicalInput.Replace(" ", "  ");

        var firstName = new FirstName(irregularSpacingInput);

        firstName.Value.Should().Be(FirstNames.JohnChristopher);
    }

    [Fact]
    public void FirstName_ShouldAllowCreation_WhenInputIsAtMinLength() {
        var minLength = FirstName.MinLength;
        var minLengthInput = TextInputs.CreateStringWithLength(minLength);

        var firstName = new FirstName(minLengthInput);

        firstName.Value.Should().Be(minLengthInput);
    }

    [Fact]
    public void FirstName_ShouldAllowCreation_WhenInputIsAtMaxLength() {
        var maxLength = FirstName.MaxLength;
        var maxLengthInput = TextInputs.CreateStringWithLength(maxLength);

        var firstName = new FirstName(maxLengthInput);

        firstName.Value.Should().Be(maxLengthInput);
    }

    [Fact]
    public void FirstName_ShouldNotAllowCreation_WhenInputIsBelowMinLength() {
        var invalidLength = FirstName.MinLength - 1;
        var tooShortInput = TextInputs.CreateStringWithLength(invalidLength);

        var act = () => new FirstName(tooShortInput);

        act.Should().Throw<InvalidLengthException>();
    }

    [Fact]
    public void FirstName_ShouldNotAllowCreation_WhenInputExceedsMaxLength() {
        var invalidLength = FirstName.MaxLength + 1;
        var tooLongInput = TextInputs.CreateStringWithLength(invalidLength);

        var act = () => new FirstName(tooLongInput);

        act.Should().Throw<InvalidLengthException>();
    }

    [Fact]
    public void FirstName_ShouldBeEqual_WhenValuesAreIdentical() {
        var input = FirstNames.Sergio;

        var firstName = new FirstName(input);
        var sameFirstName = new FirstName(input);

        firstName.Should().Be(sameFirstName);
        firstName.GetHashCode().Should().Be(sameFirstName.GetHashCode());
        (firstName == sameFirstName).Should().BeTrue();
        (firstName != sameFirstName).Should().BeFalse();
    }

    [Fact]
    public void FirstName_ShouldBeEqual_WhenValuesDifferOnlyInCase() {
        var input = FirstNames.Sergio;
        var variantInput = input.ToUpperInvariant();

        var canonicalFirstName = new FirstName(input);
        var caseVariant = new FirstName(variantInput);

        canonicalFirstName.Should().Be(caseVariant);
        canonicalFirstName.GetHashCode().Should().Be(caseVariant.GetHashCode());
        (canonicalFirstName == caseVariant).Should().BeTrue();
        (canonicalFirstName != caseVariant).Should().BeFalse();
    }

    [Fact]
    public void FirstName_ShouldBeEqual_WhenValuesDifferOnlyInLeadingOrTrailingWhiteSpace() {
        var input = FirstNames.JohnChristopher;
        var variantInput = " " + input + " ";

        var canonicalFirstName = new FirstName(input);
        var leadingOrTrailingWhiteSpaceVariant = new FirstName(variantInput);

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
        var input = FirstNames.JohnChristopher;
        var variantInput = input.Replace(" ", "  ");

        var canonicalFirstName = new FirstName(input);
        var internalWhiteSpaceVariant = new FirstName(variantInput);

        canonicalFirstName.Should().Be(internalWhiteSpaceVariant);
        canonicalFirstName.GetHashCode()
            .Should().Be(internalWhiteSpaceVariant.GetHashCode());
        (canonicalFirstName == internalWhiteSpaceVariant).Should().BeTrue();
        (canonicalFirstName != internalWhiteSpaceVariant).Should().BeFalse();
    }

    [Fact]
    public void FirstName_ShouldBeEqual_WhenValuesDifferOnlyInCaseOrWhiteSpace() {
        var canonicalInput = FirstNames.JohnChristopher;
        var leadingOrTrailingWhiteSpaceVariantInput =
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

        variations.Should().AllSatisfy(variant => {
            variant.Should().Be(canonicalFirstName);
            variant.GetHashCode().Should().Be(expectedHash);
            (variant == canonicalFirstName).Should().BeTrue();
            (variant != canonicalFirstName).Should().BeFalse();
        });
    }

    [Fact]
    public void FirstName_ShouldNotBeEqual_WhenValuesAreDifferent() {
        var firstName = FirstNames.CreateSergio();

        var otherFirstName = FirstNames.CreateJohnChristopher();

        firstName.Should().NotBe(otherFirstName);
        (firstName != otherFirstName).Should().BeTrue();
        (firstName == otherFirstName).Should().BeFalse();
    }

    [Fact]
    public void FirstName_LengthConstraint_ShouldExposeMinLength() {
        FirstName.LengthConstraint.MinLength.Should().Be(FirstName.MinLength);
    }

    [Fact]
    public void FirstName_LengthConstraint_ShouldExposeMaxLength() {
        FirstName.LengthConstraint.MaxLength.Should().Be(FirstName.MaxLength);
    }

    [Theory]
    [InlineData("Sergio", true)]
    [InlineData("  Sergio  ", true)]
    [InlineData("John  Christopher", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("A", false)]
    public void FirstName_LengthConstraint_ShouldValidateInput(
        string input,
        bool expected
    ) {
        var result = FirstName.LengthConstraint.IsSatisfiedBy(input);

        result.Should().Be(expected);
    }

    [Fact]
    public void FirstName_ShouldReturnValue_WhenConvertedToString() {
        var firstName = FirstNames.CreateSergio();

        var result = firstName.ToString();

        result.Should().Be(firstName.Value);
    }

    [Fact]
    public void FirstName_LengthLimits_ShouldMatchDomainRules() {
        FirstName.MinLength.Should().Be(2);
        FirstName.MaxLength.Should().Be(100);
    }
}
