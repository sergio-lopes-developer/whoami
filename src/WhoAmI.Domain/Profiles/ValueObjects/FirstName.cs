using WhoAmI.Domain.Profiles.Exceptions;
using WhoAmI.Domain.Shared.Base;
using WhoAmI.Domain.Shared.Constraints;
using WhoAmI.Domain.Shared.Guards;

namespace WhoAmI.Domain.Profiles.ValueObjects;

public sealed class FirstName : TextValueObject {
    public const int MinLength = 2;

    public const int MaxLength = 100;

    public string Value { get; } = null!;

    private readonly string _equalityValue = null!;

    public FirstName(string input) {
        Guard.AgainstNullOrWhiteSpace(input, nameof(input));

        var normalizedInput = Normalize(input);

        Guard.Against(
            !IsNormalizedLengthValid(normalizedInput),
            () => new InvalidLengthException(MinLength, MaxLength, "First name")
        );

        Value = normalizedInput;
        _equalityValue = Value.ToUpperInvariant();
    }

    // ReSharper disable once UnusedMember.Local
    private FirstName() { } // required by EF

    public static TextLengthConstraint LengthConstraint =>
        new(MinLength, MaxLength, HasValidLength);

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return _equalityValue;
    }

    private static bool HasValidLength(string? input) {
        if (string.IsNullOrWhiteSpace(input)) {
            return false;
        }

        var normalizedInput = Normalize(input);

        return IsNormalizedLengthValid(normalizedInput);
    }

    private static bool IsNormalizedLengthValid(string normalized) =>
        normalized.Length is >= MinLength and <= MaxLength;

    private static string Normalize(string input) => NormalizeText(input);
}
