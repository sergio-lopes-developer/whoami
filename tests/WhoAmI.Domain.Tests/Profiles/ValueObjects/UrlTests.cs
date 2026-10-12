using FluentAssertions;
using WhoAmI.Domain.Profiles.Exceptions;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Domain.Shared.Exceptions;
using WhoAmI.Testing.Helpers;
using WhoAmI.Testing.TestData.Profiles.ValueObjects;

namespace WhoAmI.Domain.Tests.Profiles.ValueObjects;

public class UrlTests {
    [Theory]
    [InlineData(Urls.LinkedIn)]
    [InlineData(Urls.GitHub)]
    [InlineData("https://example.com")]
    [InlineData("HTTPS://Example.Com")]
    public void Url_ShouldCreate_WhenInputIsValid(string input) {
        // Act
        var url = new Url(input);

        // Assert
        url.Value.Should().Be(input);
    }

    [Fact]
    public void Url_ShouldPreserveInputCase_WhenCreated() {
        // Arrange
        const string canonicalInput = Urls.LinkedIn;
        var caseVariantInput = canonicalInput.ToUpperInvariant();

        // Act
        var url = new Url(caseVariantInput);

        // Assert
        url.Value.Should().Be(caseVariantInput);
    }

    [Fact]
    public void Url_ShouldThrow_WhenInputIsNull() {
        // Act
        var act = () => new Url(null!);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [MemberData(nameof(TextInputs.Blank), MemberType = typeof(TextInputs))]
    public void Url_ShouldThrow_WhenInputIsEmptyOrWhiteSpace(
        string input
    ) {
        // Act
        var act = () => new Url(input);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(Urls.LinkedIn + " ")]
    [InlineData(" " + Urls.LinkedIn)]
    [InlineData(" " + Urls.LinkedIn + " ")]
    public void Url_ShouldTrimLeadingAndTrailingWhiteSpace_WhenInputContainsWhiteSpace(
        string leadingOrTrailingWhiteSpaceInput
    ) {
        // Arrange
        const string canonicalInput = Urls.LinkedIn;

        // Act
        var url = new Url(leadingOrTrailingWhiteSpaceInput);

        // Assert
        url.Value.Should().Be(canonicalInput);
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("www.example.com")]
    [InlineData("example.com/path")]
    [InlineData("ftp://example.com")]
    [InlineData("file://example.com")]
    [InlineData("mailto:user@example.com")]
    [InlineData("javascript:alert(1)")]
    [InlineData("http:/example.com")]
    [InlineData("https:example.com")]
    [InlineData("://example.com")]
    [InlineData("https://")]
    [InlineData("https://?")]
    [InlineData("https://#")]
    [InlineData("https://.")]
    [InlineData("Https://..")]
    [InlineData("https://example..com")]
    [InlineData("https://example .com")]
    [InlineData("https:// example.com")]
    [InlineData("https://example.com /path")]
    [InlineData("https://exa[mple].com")]
    [InlineData("not a url")]
    [InlineData("just some text")]
    [InlineData("123456")]
    [InlineData("////")]
    public void Url_ShouldThrow_WhenInputIsMalformed(string input) {
        // Act
        var act = () => new Url(input);

        // Assert
        act.Should().Throw<InvalidUrlException>();
    }

    [Theory]
    [InlineData("https://-example.com")]
    [InlineData("https://example-.com")]
    [InlineData(Urls.Example + "/<script>")]
    [InlineData(Urls.Example + "/\"test\"")]
    [InlineData(Urls.Example + "/{test}")]
    [InlineData(Urls.Example + "/pa th")]
    public void Url_ShouldThrow_WhenInputContainsInvalidCharacters(
        string input
    ) {
        // Act
        var act = () => new Url(input);

        // Assert
        act.Should().Throw<InvalidUrlException>();
    }

    [Fact]
    public void Url_ShouldBeEqual_WhenSchemeAndHostDifferOnlyInCase() {
        // Arrange
        const string canonicalInput = Urls.Example;
        var caseVariantInput = canonicalInput.ToUpperInvariant();

        var caseVariant = new Url(caseVariantInput);
        var canonicalUrl = new Url(canonicalInput);

        // Assert
        canonicalUrl.Should().Be(caseVariant);
        canonicalUrl.GetHashCode().Should().Be(caseVariant.GetHashCode());

        (canonicalUrl == caseVariant).Should().BeTrue();
        (canonicalUrl != caseVariant).Should().BeFalse();
    }

    [Fact]
    public void Url_ShouldBeEqual_WhenValuesAreEqual() {
        // Arrange
        const string input = Urls.LinkedIn;

        var url = new Url(input);
        var sameUrl = new Url(input);

        // Assert
        url.Should().Be(sameUrl);
        url.GetHashCode().Should().Be(sameUrl.GetHashCode());

        (url == sameUrl).Should().BeTrue();
        (url != sameUrl).Should().BeFalse();
    }

    [Theory]
    [InlineData("https://example.com", "https://example.com:443")]
    [InlineData("http://example.com", "http://example.com:80")]
    public void Url_ShouldBeEqual_WhenValuesDifferOnlyByDefaultPort(
        string input,
        string equivalentInput
    ) {
        // Arrange
        var canonicalUrl = new Url(equivalentInput);
        var defaultPortVariant = new Url(input);

        // Assert
        canonicalUrl.Should().Be(defaultPortVariant);

        canonicalUrl.GetHashCode()
            .Should().Be(defaultPortVariant.GetHashCode());

        (canonicalUrl == defaultPortVariant).Should().BeTrue();
        (canonicalUrl != defaultPortVariant).Should().BeFalse();
    }

    [Fact]
    public void Url_ShouldBeEqual_WhenValuesDifferOnlyInLeadingOrTrailingWhiteSpace() {
        // Arrange
        const string input = Urls.GitHub;
        const string variantInput = " " + input + " ";

        var canonicalUrl = new Url(input);
        var leadingOrTrailingWhiteSpaceVariant = new Url(variantInput);

        // Assert
        canonicalUrl.Should().Be(leadingOrTrailingWhiteSpaceVariant);

        canonicalUrl.GetHashCode()
            .Should().Be(leadingOrTrailingWhiteSpaceVariant.GetHashCode());

        (canonicalUrl == leadingOrTrailingWhiteSpaceVariant).Should().BeTrue();
        (canonicalUrl != leadingOrTrailingWhiteSpaceVariant).Should().BeFalse();
    }

    [Theory]
    [InlineData(Urls.GitHub + "/FILE", Urls.GitHub + "/file")]
    [InlineData(Urls.GitHub + "/file?name=ZIP", Urls.GitHub + "/file?name=zip")]
    public void Url_ShouldNotBeEqual_WhenPathAndQueryDifferOnlyInCase(
        string input,
        string variantInput
    ) {
        // Arrange
        var canonicalUrl = new Url(input);
        var caseVariant = new Url(variantInput);

        // Assert
        canonicalUrl.Should().NotBe(caseVariant);

        (canonicalUrl != caseVariant).Should().BeTrue();
        (canonicalUrl == caseVariant).Should().BeFalse();
    }

    [Fact]
    public void Url_ShouldNotBeEqual_WhenNonDefaultPortsDiffer() {
        // Arrange
        const string input = Urls.Example;
        const string variantInput = $"{input}:8443";

        var canonicalUrl = new Url(input);
        var nonDefaultPortVariant = new Url(variantInput);

        // Assert
        canonicalUrl.Should().NotBe(nonDefaultPortVariant);

        (canonicalUrl != nonDefaultPortVariant).Should().BeTrue();
        (canonicalUrl == nonDefaultPortVariant).Should().BeFalse();
    }

    [Theory]
    [InlineData(Urls.LinkedIn, Urls.GitHub)]
    [InlineData("https://example.com", "https://example.org")]
    public void Url_ShouldNotBeEqual_WhenValuesAreDifferent(
        string input,
        string variantInput
    ) {
        // Arrange
        var url = new Url(input);
        var otherUrl = new Url(variantInput);

        // Assert
        url.Should().NotBe(otherUrl);

        (url != otherUrl).Should().BeTrue();
        (url == otherUrl).Should().BeFalse();
    }

    [Fact]
    public void Url_ShouldReturnValue_WhenConvertedToString() {
        // Arrange
        var url = Urls.CreateLinkedIn();

        // Act
        var result = url.ToString();

        // Assert
        result.Should().Be(url.Value);
    }

    [Theory]
    [InlineData(Urls.LinkedIn)]
    [InlineData("HTTPS://Example.Com")]
    [InlineData("  HTTPS://Example.Com  ")]
    public void IsValid_ShouldReturnTrue_WhenInputIsValid(string input) {
        // Act
        var result = Url.IsValid(input);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("example.com")]
    [InlineData("www.example.com")]
    [InlineData("example.com/path")]
    [InlineData("ftp://example.com")]
    [InlineData("file://example.com")]
    [InlineData("mailto:user@example.com")]
    [InlineData("javascript:alert(1)")]
    [InlineData("http:/example.com")]
    [InlineData("https:example.com")]
    [InlineData("://example.com")]
    [InlineData("https://")]
    [InlineData("https://?")]
    [InlineData("https://#")]
    [InlineData("https://.")]
    [InlineData("Https://..")]
    [InlineData("https://example..com")]
    [InlineData("https://example .com")]
    [InlineData("https:// example.com")]
    [InlineData("https://example.com /path")]
    [InlineData("https://exa[mple].com")]
    [InlineData("not a url")]
    [InlineData("just some text")]
    [InlineData("123456")]
    [InlineData("////")]
    public void IsValid_ShouldReturnFalse_WhenInputIsInvalid(string? input) {
        // Act
        var result = Url.IsValid(input);

        // Assert
        result.Should().BeFalse();
    }
}
