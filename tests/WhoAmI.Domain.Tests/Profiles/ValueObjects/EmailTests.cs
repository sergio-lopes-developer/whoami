using FluentAssertions;
using WhoAmI.Domain.Profiles.Exceptions;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Helpers;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles.ValueObjects;

public class EmailTests {
    [Theory]
    [InlineData(Emails.Valid)]
    [InlineData("User_Name@domain.co")]
    [InlineData("a.b-c@sub.domain123.net")]
    [InlineData("user+tag@gmail.com")]
    public void Email_ShouldAllowCreation_WhenInputIsValid(string input) {
        var caseNormalizedInput = input.ToLowerInvariant();

        var email = new Email(input);

        email.Address.Should().Be(caseNormalizedInput);
    }

    [Theory]
    [InlineData(" " + Emails.Valid)]
    [InlineData(Emails.Valid + " ")]
    [InlineData(" " + Emails.Valid + " ")]
    public void Email_ShouldTrimLeadingOrTrailingWhiteSpace_WhenInputContainsIt(
        string leadingOrTrailingWhiteSpaceInput
    ) {
        var canonicalInput = Emails.Valid;

        var email = new Email(leadingOrTrailingWhiteSpaceInput);

        email.Address.Should().Be(canonicalInput);
    }

    [Fact]
    public void Email_ShouldNormalizeCase_WhenInputContainsUppercaseLetters() {
        var canonicalInput = Emails.Valid;
        var uppercaseInput = canonicalInput.ToUpperInvariant();

        var email = new Email(uppercaseInput);

        email.Address.Should().Be(canonicalInput);
    }

    [Fact]
    public void Email_ShouldTrimAndNormalizeCase_WhenInputHasWhiteSpaceAndUppercase() {
        var canonicalInput = "email.test@gmail.com";
        var variantInput = "  EMAIL.TEST@GMAIL.COM  ";

        var email = new Email(variantInput);

        email.Address.Should().Be(canonicalInput);
    }

    [Fact]
    public void Email_ShouldNotAllowCreation_WhenInputIsNull() {
        var act = () => new Email(null!);

        act.Should().Throw<DomainException>();
    }

    [Theory]
    [MemberData(nameof(TextInputs.Blank), MemberType = typeof(TextInputs))]
    public void Email_ShouldNotAllowCreation_WhenInputIsEmptyOrWhiteSpace(
        string input
    ) {
        var act = () => new Email(input);

        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("user..name@email.com")]
    [InlineData("user.@email.com")]
    [InlineData(".user@email.com")]
    [InlineData("user@.email.com")]
    [InlineData("user@email..com")]
    [InlineData("user@do_main.com")]
    [InlineData("user@domain.123")]
    [InlineData("us er@email.com")]
    [InlineData("@.")]
    [InlineData("@email.com")]
    [InlineData("test@email.")]
    [InlineData("test@.com")]
    public void Email_ShouldNotAllowCreation_WhenInputIsInvalid(string input) {
        var act = () => new Email(input);

        act.Should().Throw<InvalidEmailException>();
    }

    [Fact]
    public void Email_ShouldAllowCreation_WhenInputIsAtMaxLength() {
        // Arrange
        var domainSuffix = ".com";
        var atSuffix = "@";
        var localPart = TextInputs.CreateStringWithLength(1);
        var maxAllowedDomainLength =
            Email.MaxLength
            - domainSuffix.Length
            - atSuffix.Length
            - localPart.Length;
        var domain = TextInputs.CreateStringWithLength(maxAllowedDomainLength);
        var input = $"{localPart}{atSuffix}{domain}{domainSuffix}";

        // Act
        var email = new Email(input);

        // Assert
        email.Address.Should().Be(input);
    }

    [Fact]
    public void Email_ShouldNotAllowCreation_WhenInputExceedsMaxLength() {
        // Arrange
        var domainSuffix = ".com";
        var atSuffix = "@";
        var localPart = TextInputs.CreateStringWithLength(1);
        var exceedingDomainLength =
            Email.MaxLength
            - domainSuffix.Length
            - atSuffix.Length
            - localPart.Length
            + 1;
        var domain = TextInputs.CreateStringWithLength(exceedingDomainLength);
        var input = $"{localPart}{atSuffix}{domain}{domainSuffix}";

        // Act
        var act = () => new Email(input);

        // Assert
        act.Should().Throw<InvalidEmailException>();
    }

    [Fact]
    public void Email_ShouldNotAllowCreation_WhenLocalPartExceedsMaxLength() {
        var invalidLength = Email.LocalPartMaxLength + 1;
        var localPart = TextInputs.CreateStringWithLength(invalidLength);
        var input = $"{localPart}@example.com";

        var act = () => new Email(input);

        act.Should().Throw<InvalidEmailException>();
    }

    [Fact]
    public void Email_ShouldBeEqual_WhenAddressesAreEqual() {
        var input = Emails.Valid;

        var email = new Email(input);
        var sameEmail = new Email(input);

        email.Should().Be(sameEmail);
        email.GetHashCode().Should().Be(sameEmail.GetHashCode());
        (email == sameEmail).Should().BeTrue();
        (email != sameEmail).Should().BeFalse();
    }

    [Fact]
    public void Email_ShouldBeEqual_WhenAddressesDifferOnlyInCase() {
        var input = Emails.Valid;
        var variantInput = input.ToUpperInvariant();

        var canonicalEmail = new Email(input);
        var caseVariant = new Email(variantInput);

        canonicalEmail.Should().Be(caseVariant);
        canonicalEmail.GetHashCode().Should().Be(caseVariant.GetHashCode());
        (canonicalEmail == caseVariant).Should().BeTrue();
        (canonicalEmail != caseVariant).Should().BeFalse();
    }

    [Fact]
    public void Email_ShouldBeEqual_WhenAddressesDifferOnlyInLeadingOrTrailingWhiteSpace() {
        var input = Emails.Valid;
        var variantInput = " " + input + " ";

        var canonicalEmail = new Email(input);
        var leadingOrTrailingWhiteSpaceVariant = new Email(variantInput);

        canonicalEmail.Should().Be(leadingOrTrailingWhiteSpaceVariant);
        canonicalEmail.GetHashCode()
            .Should().Be(leadingOrTrailingWhiteSpaceVariant.GetHashCode());
        (canonicalEmail == leadingOrTrailingWhiteSpaceVariant)
            .Should().BeTrue();
        (canonicalEmail != leadingOrTrailingWhiteSpaceVariant)
            .Should().BeFalse();
    }

    [Fact]
    public void Email_ShouldBeEqual_WhenAddressesDifferOnlyInCaseOrWhiteSpace() {
        var canonicalInput = Emails.Valid;
        var leadingOrTrailingWhiteSpaceVariantInput =
            "  " + canonicalInput + "  ";
        var caseVariantInput = canonicalInput.ToUpperInvariant();
        var canonicalEmail = new Email(canonicalInput);
        var expectedHash = canonicalEmail.GetHashCode();

        var variations = new[] {
            new Email(leadingOrTrailingWhiteSpaceVariantInput),
            new Email(caseVariantInput)
        };

        variations.Should().AllSatisfy(variant => {
            variant.Should().Be(canonicalEmail);
            variant.GetHashCode().Should().Be(expectedHash);
            (variant == canonicalEmail).Should().BeTrue();
            (variant != canonicalEmail).Should().BeFalse();
        });
    }

    [Fact]
    public void Email_ShouldNotBeEqual_WhenAddressesAreDifferent() {
        var email = new Email("a@email.com");

        var otherEmail = new Email("b@email.com");

        email.Should().NotBe(otherEmail);
        (email != otherEmail).Should().BeTrue();
        (email == otherEmail).Should().BeFalse();
    }

    [Theory]
    [InlineData(Emails.Valid)]
    [InlineData("USER@EMAIL.COM")]
    [InlineData(" user@email.com ")]
    public void IsValid_ShouldReturnTrue_WhenInputIsValid(string input) {
        var result = Email.IsValid(input);

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid")]
    [InlineData("@email.com")]
    public void IsValid_ShouldReturnFalse_WhenInputIsInvalid(string? input) {
        var result = Email.IsValid(input);

        result.Should().BeFalse();
    }

    [Fact]
    public void ConstructorAndIsValid_ShouldAcceptValidInputs() {
        var input = "user@email.com";
        Email.IsValid(input).Should().BeTrue();

        var act = () => new Email(input);

        act.Should().NotThrow();
    }

    [Fact]
    public void ConstructorAndIsValid_ShouldRejectInvalidInputs() {
        var input = "user..name@email.com";
        Email.IsValid(input).Should().BeFalse();

        var act = () => new Email(input);

        act.Should().Throw<InvalidEmailException>();
    }

    [Fact]
    public void Email_ShouldReturnAddress_WhenConvertedToString() {
        var email = Emails.CreateValid();

        var result = email.ToString();

        result.Should().Be(email.Address);
    }

    [Fact]
    public void Email_LengthLimits_ShouldMatchDomainSpecification() {
        Email.LocalPartMaxLength.Should().Be(64);
        Email.MaxLength.Should().Be(254);
    }
}
