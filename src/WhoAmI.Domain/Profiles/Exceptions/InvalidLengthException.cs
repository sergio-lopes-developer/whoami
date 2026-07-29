using WhoAmI.Domain.Shared.Exceptions;

namespace WhoAmI.Domain.Profiles.Exceptions;

public sealed class InvalidLengthException : DomainException {
    public int MinLength { get; }

    public int MaxLength { get; }

    public string PropertyName { get; }

    public InvalidLengthException(
        int minLength,
        int maxLength,
        string propertyName
    ) : base(CreateMessage(minLength, maxLength, propertyName)) {
        MinLength = minLength;
        MaxLength = maxLength;
        PropertyName = propertyName;
    }

    private static string CreateMessage(
        int minLength,
        int maxLength,
        string propertyName
    ) =>
        $"{propertyName} must be between {minLength} and {maxLength}.";
}
