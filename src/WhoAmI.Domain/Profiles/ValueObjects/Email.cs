using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using WhoAmI.Domain.Profiles.Exceptions;
using WhoAmI.Domain.Shared.Base;
using WhoAmI.Domain.Shared.Guards;

namespace WhoAmI.Domain.Profiles.ValueObjects;

public sealed class Email : ValueObject {
    public const int LocalPartMaxLength = 64;

    public const int MaxLength = 254;

    private const string EmailPattern =
        @"^(?=[a-zA-Z0-9])" +
        @"[a-zA-Z0-9]+([._%+-][a-zA-Z0-9]+)*@" +
        @"[a-zA-Z0-9]+(-[a-zA-Z0-9]+)*" +
        @"(\.[a-zA-Z0-9]+(-[a-zA-Z0-9]+)*)*" +
        @"\.[a-zA-Z]{2,}$";

    private static readonly Regex _regex = new(
        EmailPattern,
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public string Address { get; }

    public Email(string input) {
        Guard.AgainstNullOrWhiteSpace(input, nameof(input));

        if (!TryValidate(input, out var normalizedInput)) {
            throw new InvalidEmailException(input);
        }

        Address = normalizedInput;
    }

    public static bool IsValid(string? input) => TryValidate(input, out _);

    public override string ToString() => Address;

    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Address;
    }

    private static bool TryValidate(
        string? input,
        [NotNullWhen(true)] out string? normalizedInput
    ) {
        if (string.IsNullOrWhiteSpace(input)) {
            normalizedInput = null;
            return false;
        }

        normalizedInput = Normalize(input);

        return
            HasValidLength(normalizedInput) &&
            HasValidLocalPartLength(normalizedInput) &&
            HasValidFormat(normalizedInput);
    }

    private static string Normalize(string input) {
        var trimmedInput = input.Trim();
        return trimmedInput.ToLowerInvariant();
    }

    private static bool HasValidLength(string email) =>
        email.Length <= MaxLength;

    private static bool HasValidLocalPartLength(string email) {
        var atIndex = email.IndexOf('@');
        return atIndex is > 0 and <= LocalPartMaxLength;
    }

    private static bool HasValidFormat(string email) => _regex.IsMatch(email);
}
