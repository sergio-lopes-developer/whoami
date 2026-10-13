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
    public void Email_ShouldCreate_WhenInputIsValid(string input) {
        // Arrange
        var caseNormalizedInput = input.ToLowerInvariant();

        // Act
        var email = new Email(input);

        // Assert
        email.Address.Should().Be(caseNormalizedInput);
    }

    [Theory]
    [InlineData(" " + Emails.Valid)]
    [InlineData(Emails.Valid + " ")]
    [InlineData(" " + Emails.Valid + " ")]
    public void Email_ShouldTrimLeadingAndTrailingWhiteSpace_WhenInputContainsWhiteSpace(
        string leadingOrTrailingWhiteSpaceInput
    ) {
        // Arrange
        const string canonicalInput = Emails.Valid;

        // Act
        var email = new Email(leadingOrTrailingWhiteSpaceInput);

        // Assert
        email.Address.Should().Be(canonicalInput);
    }

    [Fact]
    public void Email_ShouldNormalizeCase_WhenInputContainsUppercaseLetters() {
        // Arrange
        const string canonicalInput = Emails.Valid;
        var uppercaseInput = canonicalInput.ToUpperInvariant();

        // Act
        var email = new Email(uppercaseInput);

        // Assert
        email.Address.Should().Be(canonicalInput);
    }

    [Fact]
    public void Email_ShouldTrimAndNormalizeCase_WhenInputContainsWhiteSpaceAndUppercaseLetters() {
        // Arrange
        const string canonicalInput = "email.test@gmail.com";
        const string variantInput = "  EMAIL.TEST@GMAIL.COM  ";

        // Act
        var email = new Email(variantInput);

        // Assert
        email.Address.Should().Be(canonicalInput);
    }

    [Fact]
    public void Email_ShouldThrow_WhenInputIsNull() {
        // Act
        var act = () => new Email(null!);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [MemberData(nameof(TextInputs.Blank), MemberType = typeof(TextInputs))]
    public void Email_ShouldThrow_WhenInputIsEmptyOrWhiteSpace(
        string input
    ) {
        // Act
        var act = () => new Email(input);

        // Assert
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
    public void Email_ShouldThrow_WhenInputIsInvalid(string input) {
        // Act
        var act = () => new Email(input);

        // Assert
        act.Should().Throw<InvalidEmailException>();
    }

    [Fact]
    public void Email_ShouldCreate_WhenInputHasMaximumLength() {
        // Arrange
        const string domainSuffix = ".com";
        const string atSuffix = "@";
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
    public void Email_ShouldThrow_WhenInputExceedsMaximumLength() {
        // Arrange
        const string domainSuffix = ".com";
        const string atSuffix = "@";
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
    public void Email_ShouldThrow_WhenLocalPartExceedsMaximumLength() {
        // Arrange
        const int invalidLength = Email.LocalPartMaxLength + 1;
        var localPart = TextInputs.CreateStringWithLength(invalidLength);
        var input = $"{localPart}@example.com";

        // Act
        var act = () => new Email(input);

        // Assert
        act.Should().Throw<InvalidEmailException>();
    }

    [Fact]
    public void Email_ShouldBeEqual_WhenAddressesAreEqual() {
        // Arrange
        const string input = Emails.Valid;

        var email = new Email(input);
        var sameEmail = new Email(input);

        // Assert
        email.Should().Be(sameEmail);
        email.GetHashCode().Should().Be(sameEmail.GetHashCode());

        (email == sameEmail).Should().BeTrue();
        (email != sameEmail).Should().BeFalse();
    }

    [Fact]
    public void Email_ShouldBeEqual_WhenAddressesDifferOnlyInCase() {
        // Arrange
        const string input = Emails.Valid;
        var variantInput = input.ToUpperInvariant();

        var canonicalEmail = new Email(input);
        var caseVariant = new Email(variantInput);

        // Assert
        canonicalEmail.Should().Be(caseVariant);
        canonicalEmail.GetHashCode().Should().Be(caseVariant.GetHashCode());

        (canonicalEmail == caseVariant).Should().BeTrue();
        (canonicalEmail != caseVariant).Should().BeFalse();
    }

    [Fact]
    public void Email_ShouldBeEqual_WhenAddressesDifferOnlyInLeadingOrTrailingWhiteSpace() {
        // Arrange
        const string input = Emails.Valid;
        const string variantInput = " " + input + " ";

        var canonicalEmail = new Email(input);
        var leadingOrTrailingWhiteSpaceVariant = new Email(variantInput);

        // Assert
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
        // Arrange
        const string canonicalInput = Emails.Valid;

        const string leadingOrTrailingWhiteSpaceVariantInput =
            "  " + canonicalInput + "  ";

        var caseVariantInput = canonicalInput.ToUpperInvariant();
        var canonicalEmail = new Email(canonicalInput);
        var expectedHash = canonicalEmail.GetHashCode();

        var variations = new[] {
            new Email(leadingOrTrailingWhiteSpaceVariantInput),
            new Email(caseVariantInput)
        };

        // Assert
        variations.Should().AllSatisfy(variant => {
            variant.Should().Be(canonicalEmail);
            variant.GetHashCode().Should().Be(expectedHash);

            (variant == canonicalEmail).Should().BeTrue();
            (variant != canonicalEmail).Should().BeFalse();
        });
    }

    [Fact]
    public void Email_ShouldNotBeEqual_WhenAddressesAreDifferent() {
        // Arrange
        var email = new Email("a@email.com");
        var otherEmail = new Email("b@email.com");

        // Assert
        email.Should().NotBe(otherEmail);

        (email != otherEmail).Should().BeTrue();
        (email == otherEmail).Should().BeFalse();
    }

    [Fact]
    public void Email_ShouldReturnAddress_WhenConvertedToString() {
        // Arrange
        var email = Emails.CreateValid();

        // Act
        var result = email.ToString();

        // Assert
        result.Should().Be(email.Address);
    }

    [Fact]
    public void Email_ShouldHaveExpectedLengthLimits() {
        // Assert
        Email.LocalPartMaxLength.Should().Be(64);
        Email.MaxLength.Should().Be(254);
    }

    [Theory]
    [InlineData(Emails.Valid)]
    [InlineData("USER@EMAIL.COM")]
    [InlineData(" user@email.com ")]
    public void IsValid_ShouldReturnTrue_WhenInputIsValid(string input) {
        // Act
        var result = Email.IsValid(input);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid")]
    [InlineData("@email.com")]
    public void IsValid_ShouldReturnFalse_WhenInputIsInvalid(string? input) {
        // Act
        var result = Email.IsValid(input);

        // Assert
        result.Should().BeFalse();
    }
}
