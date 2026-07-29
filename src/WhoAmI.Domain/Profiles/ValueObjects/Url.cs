using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using WhoAmI.Domain.Profiles.Exceptions;
using WhoAmI.Domain.Shared.Base;
using WhoAmI.Domain.Shared.Guards;

namespace WhoAmI.Domain.Profiles.ValueObjects;

/// <summary>
/// Represents a validated HTTP/HTTPS URL with RFC 1035-compliant host name
/// rules.
/// </summary>
public sealed class Url : ValueObject {
    private const int MaxHostLength = 255;

    private const int MaxLabelInnerLength = 61;

    private const int MinTopLevelDomainLength = 2;

    private static readonly Regex _hostRegex = new(
        $$"""
        ^(?=.{1,{{MaxHostLength}}}$)
        (?:[A-Za-z0-9]
            (?:[A-Za-z0-9-]{0,{{MaxLabelInnerLength}}}[A-Za-z0-9])?\.
        )+
        [A-Za-z]{{{MinTopLevelDomainLength}},}$
        """,
        RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace
    );

    private static readonly Regex _unsafeCharactersRegex = new(
        @"[<>{}""\s]",
        RegexOptions.Compiled
    );

    public string Value { get; }

    private readonly string _equalityValue;

    public Url(string input) {
        Guard.AgainstNullOrWhiteSpace(input, nameof(input));

        if (!TryValidate(input, out var normalizedInput, out var uri)) {
            throw new InvalidUrlException(input);
        }

        Value = normalizedInput;
        _equalityValue = NormalizeUri(uri);
    }

    public static bool IsValid(string? input) =>
        TryValidate(input, out _, out _);

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return _equalityValue;
    }

    private static bool TryValidate(
        string? input,
        [NotNullWhen(true)] out string? normalizedInput,
        [NotNullWhen(true)] out Uri? uri
    ) {
        if (string.IsNullOrWhiteSpace(input)) {
            uri = null;
            normalizedInput = null;
            return false;
        }

        normalizedInput = input.Trim();

        return
            TryParseUri(normalizedInput, out uri) &&
            HasValidUri(uri, normalizedInput);
    }

    private static string NormalizeUri(Uri uri) {
        var authority = uri.Host.ToLowerInvariant();

        if (!IsDefaultPort(uri)) {
            authority += $":{uri.Port}";
        }

        return
            $"{uri.Scheme.ToLowerInvariant()}://{authority}" +
            uri.PathAndQuery;
    }

    private static bool TryParseUri(
        string input,
        [NotNullWhen(true)] out Uri? uri
    ) =>
        Uri.TryCreate(input, UriKind.Absolute, out uri);

    private static bool HasValidUri(Uri uri, string input) {
        var hasValidScheme =
            uri.Scheme == Uri.UriSchemeHttp ||
            uri.Scheme == Uri.UriSchemeHttps;

        var hasValidHost = _hostRegex.IsMatch(uri.Host);
        var hasSafeCharacters = !_unsafeCharactersRegex.IsMatch(input);

        return hasValidScheme && hasValidHost && hasSafeCharacters;
    }

    private static bool IsDefaultPort(Uri uri) =>
        (uri.Scheme == Uri.UriSchemeHttp && uri.Port == 80) ||
        (uri.Scheme == Uri.UriSchemeHttps && uri.Port == 443);
}
