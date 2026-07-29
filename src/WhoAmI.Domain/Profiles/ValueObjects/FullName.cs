using WhoAmI.Domain.Shared.Base;
using WhoAmI.Domain.Shared.Guards;

namespace WhoAmI.Domain.Profiles.ValueObjects;

public sealed class FullName : ValueObject {
    public FirstName FirstName { get; } = null!;

    public LastName LastName { get; } = null!;

    public FullName(FirstName firstName, LastName lastName) {
        Guard.AgainstNull(firstName, nameof(firstName));
        Guard.AgainstNull(lastName, nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }

    // ReSharper disable once UnusedMember.Local
    private FullName() { } // required by EF

    public override string ToString() => $"{FirstName} {LastName}";

    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return FirstName;
        yield return LastName;
    }
}
