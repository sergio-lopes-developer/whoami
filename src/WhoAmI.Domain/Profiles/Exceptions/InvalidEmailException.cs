using WhoAmI.Domain.Shared.Exceptions;

namespace WhoAmI.Domain.Profiles.Exceptions;

public sealed class InvalidEmailException : DomainException {
    public string Value { get; }

    public InvalidEmailException(string value) :
        base("Email address is invalid.") => Value = value;
}
