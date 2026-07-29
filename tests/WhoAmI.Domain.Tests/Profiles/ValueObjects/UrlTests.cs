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
    public void Url_ShouldAllowCreation_WhenInputIsValid(string input) {
        var url = new Url(input);

        url.Value.Should().Be(input);
    }

    [Fact]
    public void Url_ShouldPreserveInputCase_WhenCreated() {
        var canonicalInput = Urls.LinkedIn;
        var caseVariantInput = canonicalInput.ToUpperInvariant();

        var url = new Url(caseVariantInput);

        url.Value.Should().Be(caseVariantInput);
    }

    [Fact]
    public void Url_ShouldNotAllowCreation_WhenInputIsNull() {
        var act = () => new Url(null!);

        act.Should().Throw<DomainException>();
    }

    [Theory]
    [MemberData(nameof(TextInputs.Blank), MemberType = typeof(TextInputs))]
    public void Url_ShouldNotAllowCreation_WhenInputIsEmptyOrWhiteSpace(
        string input
    ) {
        var act = () => new Url(input);

        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(Urls.LinkedIn + " ")]
    [InlineData(" " + Urls.LinkedIn)]
    [InlineData(" " + Urls.LinkedIn + " ")]
    public void Url_ShouldTrimLeadingOrTrailingWhiteSpace_WhenInputContainsIt(
        string leadingOrTrailingWhiteSpaceInput
    ) {
        var canonicalInput = Urls.LinkedIn;

        var url = new Url(leadingOrTrailingWhiteSpaceInput);

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
    public void Url_ShouldNotAllowCreation_WhenInputIsMalformed(string input) {
        var act = () => new Url(input);

        act.Should().Throw<InvalidUrlException>();
    }

    [Theory]
    [InlineData("https://-example.com")]
    [InlineData("https://example-.com")]
    [InlineData(Urls.Example + "/<script>")]
    [InlineData(Urls.Example + "/\"test\"")]
    [InlineData(Urls.Example + "/{test}")]
    [InlineData(Urls.Example + "/pa th")]
    public void Url_ShouldNotAllowCreation_WhenInputContainsIllegalCharacters(
        string input
    ) {
        var act = () => new Url(input);

        act.Should().Throw<InvalidUrlException>();
    }

    [Fact]
    public void Url_ShouldBeCaseInsensitive_ForSchemeAndHost() {
        var canonicalInput = Urls.Example;
        var caseVariantInput = canonicalInput.ToUpperInvariant();

        var caseVariant = new Url(caseVariantInput);
        var canonicalUrl = new Url(canonicalInput);

        canonicalUrl.Should().Be(caseVariant);
        canonicalUrl.GetHashCode().Should().Be(caseVariant.GetHashCode());
        (canonicalUrl == caseVariant).Should().BeTrue();
        (canonicalUrl != caseVariant).Should().BeFalse();
    }

    [Theory]
    [InlineData(Urls.GitHub + "/FILE", Urls.GitHub + "/file")]
    [InlineData(Urls.GitHub + "/file?name=ZIP", Urls.GitHub + "/file?name=zip")]
    public void Url_ShouldBeCaseSensitive_ForPathAndQuery(
        string input,
        string variantInput
    ) {
        var canonicalUrl = new Url(input);
        var caseVariant = new Url(variantInput);

        canonicalUrl.Should().NotBe(caseVariant);
        (canonicalUrl != caseVariant).Should().BeTrue();
        (canonicalUrl == caseVariant).Should().BeFalse();
    }

    [Fact]
    public void Url_ShouldBeEqual_WhenValuesAreIdentical() {
        var input = Urls.LinkedIn;

        var url = new Url(input);
        var sameUrl = new Url(input);

        url.Should().Be(sameUrl);
        url.GetHashCode().Should().Be(sameUrl.GetHashCode());
        (url == sameUrl).Should().BeTrue();
        (url != sameUrl).Should().BeFalse();
    }

    [Theory]
    [InlineData("https://example.com", "https://example.com:443")]
    [InlineData("http://example.com", "http://example.com:80")]
    public void Url_ShouldBeEqual_WhenOnlyDefaultPortDiffers(
        string input,
        string equivalentInput
    ) {
        var canonicalUrl = new Url(equivalentInput);
        var defaultPortVariant = new Url(input);

        canonicalUrl.Should().Be(defaultPortVariant);
        canonicalUrl.GetHashCode()
            .Should().Be(defaultPortVariant.GetHashCode());
        (canonicalUrl == defaultPortVariant).Should().BeTrue();
        (canonicalUrl != defaultPortVariant).Should().BeFalse();
    }

    [Fact]
    public void Url_ShouldBeEqual_WhenValuesDifferOnlyInLeadingOrTrailingWhiteSpace() {
        var input = Urls.GitHub;
        var variantInput = " " + input + " ";

        var canonicalUrl = new Url(input);
        var leadingOrTrailingWhiteSpaceVariant = new Url(variantInput);

        canonicalUrl.Should().Be(leadingOrTrailingWhiteSpaceVariant);
        canonicalUrl.GetHashCode()
            .Should().Be(leadingOrTrailingWhiteSpaceVariant.GetHashCode());
        (canonicalUrl == leadingOrTrailingWhiteSpaceVariant).Should().BeTrue();
        (canonicalUrl != leadingOrTrailingWhiteSpaceVariant).Should().BeFalse();
    }

    [Fact]
    public void Url_ShouldNotBeEqual_WhenNonDefaultPortsDiffer() {
        var input = Urls.Example;
        var variantInput = $"{input}:8443";

        var canonicalUrl = new Url(input);
        var nonDefaultPortVariant = new Url(variantInput);

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
        var url = new Url(input);
        var otherUrl = new Url(variantInput);

        url.Should().NotBe(otherUrl);
        (url != otherUrl).Should().BeTrue();
        (url == otherUrl).Should().BeFalse();
    }

    [Theory]
    [InlineData(Urls.LinkedIn)]
    [InlineData("HTTPS://Example.Com")]
    [InlineData("  HTTPS://Example.Com  ")]
    public void IsValid_ShouldReturnTrue_WhenInputIsValid(string input) {
        var result = Url.IsValid(input);

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
        var result = Url.IsValid(input);

        result.Should().BeFalse();
    }

    [Fact]
    public void ConstructorAndIsValid_ShouldAcceptValidInputs() {
        var input = Urls.LinkedIn;
        Url.IsValid(input).Should().BeTrue();

        var act = () => new Url(input);

        act.Should().NotThrow();
    }

    [Fact]
    public void ConstructorAndIsValid_ShouldRejectInvalidInputs() {
        var input = "www.example.com";
        Url.IsValid(input).Should().BeFalse();

        var act = () => new Url(input);

        act.Should().Throw<InvalidUrlException>();
    }

    [Fact]
    public void Url_ShouldReturnValue_WhenConvertedToString() {
        var url = Urls.CreateLinkedIn();

        var result = url.ToString();

        result.Should().Be(url.Value);
    }
}
