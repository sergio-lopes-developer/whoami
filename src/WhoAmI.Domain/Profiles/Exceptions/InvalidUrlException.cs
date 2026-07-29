using WhoAmI.Domain.Shared.Exceptions;

namespace WhoAmI.Domain.Profiles.Exceptions;

public sealed class InvalidUrlException : DomainException {
    public string Value { get; }

    public InvalidUrlException(string value) :
        base("Invalid URL.") => Value = value;
}
